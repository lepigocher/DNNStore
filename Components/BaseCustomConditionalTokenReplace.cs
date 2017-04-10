using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Reflection;
using System.Text.RegularExpressions;

using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Components
{
    public abstract class BaseCustomConditionalTokenReplace : BaseCustomTokenReplace
    {
        #region Private Members

        // Regular expression patterns
        private const string NestedStatements = @"\[(?<Keyword>\w+?)\(\s*?(?<Condition>.*?)\s*?\)\](?<Content>(?>\[\w+?\(.*?\)\](?<DEPTH>)|\[/\w+?\](?<-DEPTH>)|.?)*(?(DEPTH)(?!)))\s*?\[/\k<Keyword>\][\r\n]*?";
        private const string IfCondition = @"(?<ObjectName>[\w.]+?)\s*(?<Operator>[!=<>]{1,2})\s*(?<CompareValue>.+)";
        private const string ForEachCondition = @"(?<ObjectName>\w+?)\s+IN\s+(?<ObjectSource>[\w.]+)(?:\[(?<Min>[\d]+?)-(?<Max>[\d]+?)\])?";
        private const string WithCondition = @"(?<ObjectSource>[\w.\[\]]+?)\s+AS\s+(?<ObjectName>\w+)";
        private const string IndexCondition = @"(?<Name>\w+)\[(?<Index>\d+)\]";
        // Error message template
        private const string ConditionError = "The condition of '[{0}({1})]' is not valid!";
        // ICollectionFullName
        private readonly string _collectionTypeFullName = typeof (ICollection<>).FullName;

        #endregion

        #region Properties

        /// <summary>
        /// Public Dictionary of collection source data.
        /// </summary>
        protected Dictionary<string, IEnumerable<IPropertyAccess>> CollectionSource = new Dictionary<string, IEnumerable<IPropertyAccess>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Protected event called when an unknow statement is found.
        /// </summary>
        protected event EventHandler<EvalStatementEventArgs> EvalStatement;

        #endregion

        #region Methods

        /// <summary>
        /// Replace tokens in the specified template.
        /// </summary>
        /// <param name="sourceText">Source Text of the template</param>
        /// <returns>Text with replacements</returns>
        protected override string ReplaceTokens(string sourceText)
        {
            return ParseStatements(sourceText);
        }

        /// <summary>
        /// Callback code registred for the EvalStatement event when an unkown statement is found.
        /// </summary>
        /// <param name="e">Eval Statement Arguments</param>
        protected virtual void OnEvalStatement(EvalStatementEventArgs e)
        {
            if (EvalStatement != null) EvalStatement(this, e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parse the specifed source text for statememts and tokens.
        /// </summary>
        /// <param name="sourceText">Source text</param>
        /// <returns>Text with replacements</returns>
        private string ParseStatements(string sourceText)
        {
            if (string.IsNullOrEmpty(sourceText))
                return string.Empty;

            // Search the most outer statement and replace them with the specified content
            // or return the source text if no statement is found.
            string result = Regex.Replace(sourceText, NestedStatements, EvaluateStatement, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Call the base class to replace remaining tokens
            return base.ReplaceTokens(result);
        }

        /// <summary>
        /// Evaluate the statement found and return the content if the condition is verified
        /// or return and empty string.
        /// </summary>
        /// <param name="m">Matched Statement</param>
        /// <returns>Evaluated Result</returns>
        private string EvaluateStatement(Match m)
        {
            string keyword = m.Groups["Keyword"].Value;
            string condition = m.Groups["Condition"].Value;
            string content = m.Groups["Content"].Value.Trim(new[] { '\r', '\n' });
            string eval = string.Empty;
            bool hasError = false;

            switch (keyword.ToLower())
            {
                case "if":
                    if (EvalIf(condition, out hasError))
                        eval = ParseStatements(content);
                    break;
                case "foreach":
                    string forObjectName;
                    IEnumerable<IPropertyAccess> forCollection;

                    if (EvalForEach(condition, out hasError, out forObjectName, out forCollection))
                    {
                        foreach (IPropertyAccess forObject in forCollection)
                        {
                            PropertySource[forObjectName] = forObject;
                            eval += ParseStatements(content);
                        }
                        PropertySource[forObjectName] = null;
                    }
                    break;
                case "with":
                    string withObjectName;
                    object withObjectFound;

                    if (EvalWith(condition, out hasError, out withObjectName, out withObjectFound))
                    {
                        if (withObjectFound is IEnumerable<IPropertyAccess>)
                            CollectionSource[withObjectName] = withObjectFound as IEnumerable<IPropertyAccess>;
                        else if (withObjectFound is IPropertyAccess)
                            PropertySource[withObjectName] = withObjectFound as IPropertyAccess;

                        eval = ParseStatements(content);

                        if (withObjectFound is IEnumerable<IPropertyAccess>)
                            CollectionSource[withObjectName] = null;
                        else if (withObjectFound is IPropertyAccess)
                            PropertySource[withObjectName] = null;
                    }
                    break;
                case "isinroles":
                    if (EvalIsInRoles(condition, out hasError))
                        eval = ParseStatements(content);
                    break;
                default:
                    EvalStatementEventArgs evalArgs = new EvalStatementEventArgs(keyword, condition, content);

                    OnEvalStatement(evalArgs);

                    if (evalArgs.IsValid)
                        eval = evalArgs.ParseContent ? ParseStatements(content) : evalArgs.Value;
                    break;
            }

            if (DebugMessages && hasError)
                eval = string.Format(ConditionError, keyword, condition);

            return eval;
        }

        /// <summary>
        /// Search for the specified object name inside PropertySource and Collection Source.
        /// Multipart can be used e.g.: MyObject.MyProperty.
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns>An object or null if the name is not found</returns>
        internal object GetObjectValue(string name)
        {
            object value = null;
            string[] parts = name.ToLower().Split('.');
            string objectName = parts[0];

            if (PropertySource.ContainsKey(objectName))
            {
                object objectFound = PropertySource[objectName];
                int count = parts.Length;

                if (count > 1)
                {
                    Type objectType = objectFound.GetType();
                    PropertyInfo property = null;

                    for (int i = 1; i < count; i++)
                    {
                        string namePart = parts[i];
                        Match matchCondition = Regex.Match(namePart, IndexCondition, RegexOptions.IgnoreCase);

                        if (matchCondition.Success)
                        {
                            namePart = matchCondition.Groups["Name"].Value;
                            int index = int.Parse(matchCondition.Groups["Index"].Value);
                            property = SingleOrDefault(namePart, objectType.GetProperties());

                            if (property != null && property.PropertyType.GetInterface(_collectionTypeFullName) != null)
                            {
                                ICollection collection = property.GetValue(objectFound, null) as ICollection;

                                if (collection != null)
                                {
                                    int j = 0;
                                    foreach (object item in collection)
                                    {
                                        if (j == index)
                                        {
                                            value = item;
                                            break;
                                        }
                                        j++;
                                    }
                                    if (value != null)
                                        property = null;
                                }
                            }
                        }
                        else if (namePart == "count" && property != null && property.PropertyType.GetInterface(_collectionTypeFullName) != null)
                        {
                            ICollection collection = property.GetValue(objectFound, null) as ICollection;

                            if (collection != null)
                                return collection.Count;
                        }
                        else
                        {
                            property = SingleOrDefault(namePart, objectType.GetProperties());

                            if (property != null)
                                objectType = property.GetType();
                        }
                    }

                    if (property != null)
                        value = property.GetValue(objectFound, null);
                }
                else
                    value = objectFound;
            }
            else if (CollectionSource.ContainsKey(objectName))
                value = CollectionSource[objectName];

            return value;
        }

        private PropertyInfo SingleOrDefault(string name, PropertyInfo[] properties)
        {
            PropertyInfo found = null;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    found = property;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Evaluate the condition of the If statement.
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="hasError">Set as true if the condition can't be evaluated</param>
        /// <returns>True if the condition is verified</returns>
        private bool EvalIf(string condition, out bool hasError)
        {
            bool eval = false;
            hasError = true;
            Match matchCondition = Regex.Match(condition, IfCondition, RegexOptions.IgnoreCase);

            if (matchCondition.Success)
            {
                object objectValue = GetObjectValue(matchCondition.Groups["ObjectName"].Value.ToLower());

                if (objectValue != null)
                {
                    Type objectType = objectValue.GetType();
                    TypeConverter valueConverter = TypeDescriptor.GetConverter(objectType);
                    string compareValue = matchCondition.Groups["CompareValue"].Value.Trim(new[] { '"' });
                    object typedValue = null;

                    // Normaly all types can convert from string but the string value have to be a valid candidate
                    if (valueConverter.CanConvertFrom(compareValue.GetType()) && valueConverter.IsValid(compareValue))
                        typedValue = valueConverter.ConvertFrom(compareValue);

                    if (typedValue != null)
                    {
                        hasError = false;

                        switch (matchCondition.Groups["Operator"].Value)
                        {
                            case "=":
                            case "==":
                                eval = objectValue.Equals(typedValue);
                                break;
                            case "<>":
                            case "!=":
                                eval = !objectValue.Equals(typedValue);
                                break;
                            case ">":
                                if (objectValue is IComparable)
                                    eval = (objectValue as IComparable).CompareTo(typedValue) > 0;
                                else
                                    hasError = true;
                                break;
                            case "<":
                                if (objectValue is IComparable)
                                    eval = (objectValue as IComparable).CompareTo(typedValue) < 0;
                                else
                                    hasError = true;
                                break;
                            case ">=":
                                if (objectValue is IComparable)
                                    eval = (objectValue as IComparable).CompareTo(typedValue) >= 0;
                                else
                                    hasError = true;
                                break;
                            case "<=":
                                if (objectValue is IComparable)
                                    eval = (objectValue as IComparable).CompareTo(typedValue) <= 0;
                                else
                                    hasError = true;
                                break;
                        }
                    }
                }
            }

            return eval;
        }

        /// <summary>
        /// Evaluate the condition of the ForEach statement.
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="hasError">Set as true if the condition can't be evaluated</param>
        /// <param name="objectName">Object name found as the left operand</param>
        /// <param name="collection">Collection found in the CollectionSource</param>
        /// <returns>True if the condition is verified</returns>
        private bool EvalForEach(string condition, out bool hasError, out string objectName, out IEnumerable<IPropertyAccess> collection)
        {
            bool eval = false;
            hasError = true;
            objectName = string.Empty;
            collection = null;
            Match matchCondition = Regex.Match(condition, ForEachCondition, RegexOptions.IgnoreCase);

            if (matchCondition.Success)
            {
                collection = GetObjectValue(matchCondition.Groups["ObjectSource"].Value) as IEnumerable<IPropertyAccess>;

                if (collection != null)
                {
                    eval = true;
                    hasError = false;
                    objectName = matchCondition.Groups["ObjectName"].Value.ToLower();

                    // Do we have indices? e.g.: collName[2-7]
                    if (matchCondition.Groups["Min"].Success && matchCondition.Groups["Max"].Success)
                    {
                        int min;
                        int max;

                        if (Int32.TryParse(matchCondition.Groups["Min"].Value, out min) &&
                            Int32.TryParse(matchCondition.Groups["Max"].Value, out max))
                        {
                            if (min <= max)
                            {
                                try
                                {
                                    --min;
                                    collection = ExtractElements(collection, min, (max - min));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }

            return eval;
        }

        private IEnumerable<IPropertyAccess> ExtractElements(IEnumerable<IPropertyAccess> source, int top, int count)
        {
            if (count <= 0)
                yield break;

            int counter = 0;

            foreach (IPropertyAccess element in source)
            {
                if (counter > top)
                    yield return element;

                if (++counter == count)
                    yield break;
            }
        }

        /// <summary>
        /// Evaluate the condition of the With statement.
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="hasError">Set as true if the condition can't be evaluated</param>
        /// <param name="objectName">Object name found as the right operand</param>
        /// <param name="objectFound">Object found inside CollectionSource or PropertySource</param>
        /// <returns>True if the condition is verified</returns>
        private bool EvalWith(string condition, out bool hasError, out string objectName, out object objectFound)
        {
            bool eval = false;
            hasError = true;
            objectName = string.Empty;
            objectFound = null;
            Match matchCondition = Regex.Match(condition, WithCondition, RegexOptions.IgnoreCase);

            if (matchCondition.Success)
            {
                objectFound = GetObjectValue(matchCondition.Groups["ObjectSource"].Value);

                if (objectFound != null && (objectFound is IPropertyAccess || objectFound is IEnumerable<IPropertyAccess>))
                {
                    eval = true;
                    hasError = false;
                    objectName = matchCondition.Groups["ObjectName"].Value.ToLower();
                }
            }

            return eval;
        }

        /// <summary>
        /// Evaluate the condition of the IsInRoles statement.
        /// </summary>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="hasError">Set as true if the condition can't be evaluated</param>
        /// <returns>True if the condition is verified</returns>
        private bool EvalIsInRoles(string condition, out bool hasError)
        {
            bool eval = false;
            hasError = true;

            if (AccessingUser != null)
            {
                string[] roleNames = condition.Trim(new[] {'"'}).Split(new[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string roleName in roleNames)
                {
                    if (AccessingUser.IsInRole(roleName.Trim()))
                    {
                        eval = true;
                        hasError = false;
                        break;
                    }
                }
            }

            return eval;
        }

        #endregion
    }
}