using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

using Xunit;

namespace Kigg.Web.Test
{
    public class jQueryScriptManagerFixture
    {
        private readonly IList<string> _scriptSources;
        private readonly IList<string> _onReadyStatements;
        private readonly IList<string> _onDisposeStatements;
        private readonly IList<string> _scriptBlocks;

        private readonly jQueryScriptManagerTestDouble _scriptManager;

        public jQueryScriptManagerFixture()
        {
            _scriptSources = new List<string>();
            _onReadyStatements = new List<string>();
            _onDisposeStatements = new List<string>();
            _scriptBlocks = new List<string>();

            _scriptManager = new jQueryScriptManagerTestDouble(_scriptSources, _onReadyStatements, _onDisposeStatements, _scriptBlocks);
        }

        [Fact]
        public void Should_Not_Able_To_Set_Visible_To_False()
        {
            _scriptManager.Visible = false;

            Assert.True(_scriptManager.Visible);
        }

        [Fact]
        public void Current_Is_Null_When_Not_Running_In_WebServer()
        {
            Assert.Null(jQueryScriptManager.Current);
        }

        [Fact]
        public void OnInit()
        {
            _scriptManager.OnInitForTest();
        }

        [Fact]
        public void RegisterSource_Should_Add_In_Source_List()
        {
            _scriptManager.RegisterSource("foobar");

            Assert.Equal("foobar", _scriptSources[_scriptSources.Count - 1]);
        }

        [Fact]
        public void RegisterSource_With_Order_Should_Add_In_Source_List_At_Specifed_Order()
        {
            _scriptManager.RegisterSource(0, "foobar");

            Assert.Equal("foobar", _scriptSources[0]);
        }

        [Fact]
        public void RegisterOnReady_Should_Add_In_OnReady_List()
        {
            _scriptManager.RegisterOnReady("foobar");

            Assert.Equal("foobar", _onReadyStatements[_onReadyStatements.Count - 1]);
        }

        [Fact]
        public void RegisterOnReady_With_Order_Should_Add_In_OnReady_List_At_Specifed_Order()
        {
            _scriptManager.RegisterOnReady(0, "foobar");

            Assert.Equal("foobar", _onReadyStatements[0]);
        }

        [Fact]
        public void RegisterBlock_Should_Add_In_Block_List()
        {
            _scriptManager.RegisterBlock("foobar");

            Assert.Equal("foobar", _scriptBlocks[_scriptBlocks.Count - 1]);
        }

        [Fact]
        public void RegisterBlock_With_Order_Should_Add_In_Block_List_At_Specifed_Order()
        {
            _scriptManager.RegisterBlock(0, "foobar");

            Assert.Equal("foobar", _scriptBlocks[0]);
        }

        [Fact]
        public void RegisterOnDispose_Should_Add_In_Dispose_List()
        {
            _scriptManager.RegisterOnDispose("foobar");

            Assert.Equal("foobar", _onDisposeStatements[_onDisposeStatements.Count - 1]);
        }

        [Fact]
        public void RegisterOnDispose_With_Order_Should_Add_In_Dispose_List_At_Specifed_Order()
        {
            _scriptManager.RegisterOnDispose(0, "foobar");

            Assert.Equal("foobar", _onDisposeStatements[0]);
        }

        [Fact]
        public void Render_Should_Write_Scripts()
        {
            StringBuilder output = new StringBuilder();

            using(StringWriter writer = new StringWriter(output))
            {
                using(HtmlTextWriter htw = new HtmlTextWriter(writer))
                {
                    _scriptManager.RegisterSource("/assets.axd?name=js1&v=1.5.0.1");

                    _scriptManager.RegisterOnReady("Search.init();");
                    _scriptManager.RegisterOnDispose("Membership.dispose();");

                    _scriptManager.RegisterBlock("function test1{alert('Alert')}");

                    _scriptManager.RenderForTest(htw);
                }
            }

            Assert.Contains("src=\"/assets.axd?name=js1&amp;v=1.5.0.1\"", output.ToString());
            Assert.Contains("Search.init();", output.ToString());
            Assert.Contains("Membership.dispose();", output.ToString());
            Assert.Contains("function test1{alert('Alert')}", output.ToString());
        }
    }

    public class jQueryScriptManagerTestDouble: jQueryScriptManager
    {
        public jQueryScriptManagerTestDouble(IList<string> scriptSources, IList<string> onReadyStatements, IList<string> onDisposeStatements, IList<string> scriptBlocks) : base(scriptSources, onReadyStatements, onDisposeStatements, scriptBlocks)
        {
        }

        public void OnInitForTest()
        {
            base.OnInit(EventArgs.Empty);
        }

        public void RenderForTest(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        public new bool DesignMode
        {
            get
            {
                return false;
            }
        }
    }
}