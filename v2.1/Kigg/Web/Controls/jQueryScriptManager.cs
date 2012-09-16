namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:jQueryScriptManager runat=\"server\"></{0}:jQueryScriptManager>")]
    public class jQueryScriptManager : Control
    {
        private readonly IList<string> _scriptSources = new List<string>();
        private readonly IList<string> _onReadyStatements = new List<string>();
        private readonly IList<string> _onDisposeStatements = new List<string>();
        private readonly IList<string> _scriptBlocks = new List<string>();

        private static readonly Type _type = typeof(jQueryScriptManager);

        public jQueryScriptManager()
        {
        }

        public jQueryScriptManager(IList<string> scriptSources, IList<string> onReadyStatements, IList<string> onDisposeStatements, IList<string> scriptBlocks) : this()
        {
            Check.Argument.IsNotNull(scriptSources, "scriptSources");
            Check.Argument.IsNotNull(onReadyStatements, "onReadyStatements");
            Check.Argument.IsNotNull(onDisposeStatements, "onDisposeStatements");
            Check.Argument.IsNotNull(scriptBlocks, "scriptBlocks");

            _scriptSources = scriptSources;
            _onReadyStatements = onReadyStatements;
            _onDisposeStatements = onDisposeStatements;
            _scriptBlocks = scriptBlocks;
        }

        public static jQueryScriptManager Current
        {
            get
            {
                return (HttpContext.Current == null) ? null : HttpContext.Current.Items[_type] as jQueryScriptManager;
            }

            private set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[_type] = value;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Visible
        {
            get { return base.Visible; }
            set { }
        }

        public void RegisterSource(string source)
        {
            RegisterSource(-1, source);
        }

        public void RegisterSource(int order, string source)
        {
            Register(_scriptSources, order, source, "source");
        }

        public void RegisterOnReady(string statement)
        {
            RegisterOnReady(-1, statement);
        }

        public void RegisterOnReady(int order, string statement)
        {
            Register(_onReadyStatements, order, statement, "statement");
        }

        public void RegisterBlock(string script)
        {
            RegisterBlock(-1, script);
        }

        public void RegisterBlock(int order, string script)
        {
            Register(_scriptBlocks, order, script, "script");
        }

        public void RegisterOnDispose(string statement)
        {
            RegisterOnDispose(-1, statement);
        }

        public void RegisterOnDispose(int order, string statement)
        {
            Register(_onDisposeStatements, order, statement, "statement");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!DesignMode)
            {
                if (Current != null)
                {
                    throw new InvalidOperationException("Cannot have more than one Script Manager.");
                }

                Current = this;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (string source in _scriptSources)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, source);
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.RenderEndTag();
            }

            StringBuilder scripts = new StringBuilder();

            if (_onReadyStatements.Count > 0)
            {
                scripts.AppendLine("$(document).ready(function(){");

                foreach (string statement in _onReadyStatements)
                {
                    scripts.AppendLine(statement);
                }

                scripts.AppendLine("});");
            }

            if (_scriptBlocks.Count > 0)
            {
                foreach (string block in _scriptBlocks)
                {
                    scripts.AppendLine(block);
                }
            }

            if (_onDisposeStatements.Count > 0)
            {
                scripts.AppendLine("$(window).unload(function(){");

                foreach (string statement in _onDisposeStatements)
                {
                    scripts.AppendLine(statement);
                }

                scripts.AppendLine("});");
            }

            if (scripts.Length > 0)
            {
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.WriteLine("//<![CDATA[");
                writer.Write(scripts.ToString());
                writer.WriteLine("//]]>");
                writer.RenderEndTag();
            }

            base.Render(writer);
        }

        private static void Register(IList<string> target, int order, string parameter, string parameterName)
        {
            Check.Argument.IsNotEmpty(parameter, parameterName);

            if (order > -1)
            {
                target.Insert(order, parameter);
            }
            else
            {
                target.Add(parameter);
            }
        }
    }
}