using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using BlogEngine.Core;

/// <summary>
/// Serializable object that holds extension,
/// extension attributes and methods
/// </summary>
[Serializable()]
public class ManagedExtension
{
  #region Private members
  string _name = string.Empty;
  string _version = string.Empty;
  string _description = string.Empty;
  bool _enabled = true;
  string _author = string.Empty;
  string _adminPage = string.Empty;
  List<ExtensionSettings> _settings = null;
  bool _showSettings = true;
  #endregion

  #region Constructor
  /// <summary>
  /// Default constructor required for serialization
  /// </summary>
  public ManagedExtension() { }
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="name">Extension Name</param>
  /// <param name="version">Extension Version</param>
  /// <param name="desc">Description</param>
  /// <param name="author">Extension Author</param>
  /// <param name="adminpage">Custom admin page for extension</param>
  public ManagedExtension(string name, string version, string desc, string author)
  {
    _name = name;
    _version = version;
    _description = desc;
    _author = author;
    _settings = new List<ExtensionSettings>();
    _enabled = true;
    _showSettings = true;
  }
  #endregion

  #region Public Serializable
  /// <summary>
  /// Extension Name
  /// </summary>
  [XmlAttribute]
  public string Name { get { return _name; } set { _name = value; } }
  /// <summary>
  /// Extension Version
  /// </summary>
  [XmlElement]
  public string Version { get { return _version; } set { _version = value; } }
  /// <summary>
  /// Extension Description
  /// </summary>
  [XmlElement]
  public string Description { get { return _description; } set { _description = value; } }
  /// <summary>
  /// Extension Author. Will show up in the settings page, can be used as a 
  /// link to author's home page 
  /// </summary>
  [XmlElement]
  public string Author { get { return _author; } set { _author = value; } }
  /// <summary>
  /// Custom admin page. If defined, link to default settings
  /// page will be replaced by the link to this page in the UI
  /// </summary>
  [XmlElement]
  public string AdminPage { get { return _adminPage; } set { _adminPage = value; } }
  /// <summary>
  /// Defines if extension is enabled.
  /// </summary>
  [XmlElement]
  public bool Enabled { get { return _enabled; } set { _enabled = value; } }
  /// <summary>
  /// Settings for the extension
  /// </summary>
  [XmlElement(IsNullable = true)]
  public List<ExtensionSettings> Settings 
  { 
      get 
      { 
          if(_settings != null)
            _settings.Sort(delegate(ExtensionSettings s1, ExtensionSettings s2)
            {return string.Compare(s1.Index.ToString(), s2.Index.ToString());});
          return _settings;
      } 
      set { _settings = value; } 
  }
  /// <summary>
  /// Show or hide settings in the admin/extensions list
  /// </summary>
  [XmlElement]
  public bool ShowSettings { get { return _showSettings; } set { _showSettings = value; } }

  #endregion

  #region Public methods
  /// <summary>
  /// Method to cache and serialize settings object
  /// </summary>
  /// <param name="settings">Settings object</param>
  public void SaveSettings(ExtensionSettings settings)
  {
    if (string.IsNullOrEmpty(settings.Name))
      settings.Name = _name;

    foreach (ExtensionSettings setItem in _settings)
    {
      if (setItem.Name == settings.Name)
      {
        _settings.Remove(setItem);
        break;
      }
    }
    _settings.Add(settings);
  }
  public void InitializeSettings(ExtensionSettings settings)
  {
    settings.Index = _settings.Count;
    SaveSettings(settings);
  }
  
  /// <summary>
  /// Determine if settings has been initialized with default
  /// values (first time new extension loaded into the manager)
  /// </summary>
  /// <param name="settingName">Settings name</param>
  /// <returns>True if initialized</returns>
  public bool Initialized(string settingName)
  {
    if (_settings != null)
    {
      foreach (ExtensionSettings setItem in _settings)
      {
        if (setItem.Name == settingName)
        {
          return true;
        }
      }
    }
    return false;
  }
  /// <summary>
  /// Method to find out if extension has setting with this name
  /// </summary>
  /// <param name="settingName">Setting Name</param>
  /// <returns>True if settings with this name already exists</returns>
  public bool ContainsSetting(string settingName)
  {
    foreach (ExtensionSettings xset in _settings)
    {
      if (xset.Name == settingName)
      {
        return true;
      }
    }
    return false;
  }

  #endregion
}