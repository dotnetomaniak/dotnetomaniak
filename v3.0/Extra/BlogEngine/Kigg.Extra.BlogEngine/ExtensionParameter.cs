using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Collections.Generic;

/// <summary>
/// Enumeration for parameter data types
/// </summary>
[Flags()]
public enum ParameterType { String, Boolean, Integer, Long, Float, Double, Decimal, DropDown, ListBox, RadioGroup }

/// <summary>
/// Extension Parameter - serializable object
/// that holds parameter attributes and collection
/// of values
/// </summary>
[Serializable()]
public class ExtensionParameter
{
  #region Private members
  string _name = string.Empty;
  string _label = string.Empty;
  int _maxLength = 100;
  bool _required = false;
  bool _keyField = false;
  StringCollection _values = null;
  ParameterType _type = ParameterType.String;
  string _selected = string.Empty;
  #endregion

  #region Public Serializable
  /// <summary>
  /// Parameter Name, often used as ID in the UI
  /// </summary>
  [XmlElement]
  public string Name { get { return _name; } set { _name = value.Trim().Replace(" ", ""); } }
  /// <summary>
  /// Used as label in the UI controls
  /// </summary>
  [XmlElement]
  public string Label { get { return _label; } set { _label = value; } }
  /// <summary>
  /// Maximum number of characters stored in the value fields
  /// </summary>
  [XmlElement]
  public int MaxLength { get { return _maxLength; } set { _maxLength = value; } }
  /// <summary>
  /// Specifies if values for parameter required
  /// </summary>
  [XmlElement]
  public bool Required { get { return _required; } set { _required = value; } }
  /// <summary>
  /// Primary Key field
  /// </summary>
  [XmlElement]
  public bool KeyField { get { return _keyField; } set { _keyField = value; } }
  /// <summary>
  /// Collection of values for given parameter
  /// </summary>
  [XmlElement]
  public StringCollection Values { get { return _values; } set { _values = value; } }
  /// <summary>
  /// Parameter Type
  /// </summary>
  //[XmlElement(IsNullable = true)]
  [XmlElement]
  public ParameterType ParamType { get { return _type; } set { _type = value; } }
  /// <summary>
  /// Selected value in parameter lists (dropdown, listbox etc.)
  /// </summary>
  [XmlElement]
  public string SelectedValue { get { return _selected; } set { _selected = value; } }
  #endregion

  #region Constructors
  /// <summary>
  /// Default constructor required for serialization
  /// </summary>
  public ExtensionParameter() { }
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="name">Parameter Name</param>
  public ExtensionParameter(string name)
  {
    _name = name;
  }
  #endregion

  #region Public Methods
  /// <summary>
  /// Add single value to value collection
  /// </summary>
  /// <param name="val">Value</param>
  public void AddValue(string val)
  {
    if (_values == null)
      _values = new StringCollection();

    _values.Add(val);
  }
  /// <summary>
  /// Add single value to collection
  /// </summary>
  /// <param name="val">Value object</param>
  public void AddValue(object val)
  {
    if (_values == null)
      _values = new StringCollection();

    _values.Add(val.ToString());
  }
  /// <summary>
  /// Update value for scalar (single value) parameter
  /// </summary>
  /// <param name="val">Value</param>
  public void UpdateScalarValue(string val)
  {
    if (_values == null || _values.Count == 0)
      AddValue(val);
    else
      _values[0] = val;
  }
  /// <summary>
  /// Update value for scalar (single value) parameter
  /// </summary>
  /// <param name="val">Value</param>
  public void UpdateScalarValue(object val)
  {
    if (_values == null || _values.Count == 0)
      AddValue(val);
    else
      _values[0] = val.ToString();
  }
  /// <summary>
  /// Delete value in parameter value collection
  /// </summary>
  /// <param name="rowIndex">Index</param>
  public void DeleteValue(int rowIndex)
  {
    _values.RemoveAt(rowIndex);
  }
  #endregion
}
