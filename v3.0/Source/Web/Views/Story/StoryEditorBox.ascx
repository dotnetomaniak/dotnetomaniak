<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewData>" %>
<div id="storyEditorBox" class="modalBox">
    <div class="titleContainer">
        <div class="title">Edytuj artykuł</div>
        <div id="storyEditorClose" class="closeButton" title="Close"></div>
    </div>
    <div class="contentContainer">
        <form id="frmStoryUpdate" action="<%= Url.Action("Update", "Story") %>" method="post">
            <div class="box">
                <input type="hidden" id="hidStoryId" name="id"/>  
                <% if (Model.CurrentUser.IsAdministrator())
                   { %>                 
                <div class="add-article-row">
                    <label for="txtStoryName" class="label">Nazwa:</label>
                    <input id="txtStoryName" name="name" type="text" class="largeTextBox" />
                    <span class="error"></span>
                </div>        
                <div class="add-article-row">
                    <label for="txtStoryCreatedAt" class="label">Utworzone:</label>
                    <input id="txtStoryCreatedAt" name="createdAt" type="text" class="largeTextBox"/>
                    <span class="error"></span>
                </div>
                <% } %>
                <div class="add-article-row">
                    <label for="txtStoryTitle" class="label">Tytuł:</label>
                    <input id="txtStoryTitle" name="title" type="text" class="largeTextBox"/>
                    <span class="error"></span>
                </div>
                <div class="add-article-row">
                    <input type="hidden" id="hidDescription2" name="description" />
                    <label for="txtStoryDescription2" class="label">Opis:</label>
                    <textarea id="txtStoryDescription2" name="description" cols="20" rows="4" class="largeTextArea"></textarea>
                    <span class="error"></span>
                </div>
                <div>
            <div class="wysiwyg-code">
                <div id="storyPreview2" class="livePreview wysiwyg-code2">
                </div>
            </div>
            <a id="lnkStoryPreview" class="actionLink hide-sample" href="javascript:void(0)">ukryj
                podgląd</a>
        </div>
                <div class="add-article-row">
                    <label for="txtStoryTags" class="label">Tagi:</label>
                    <input id="txtStoryTags" name="tags" type="text" class="largeTextBox"/>
                </div>
                <div class="add-article-row">
                    <label class="label">Kategoria:</label>
                    <% Html.RenderAction("RadioButtonList", "Category"); %>
                    <span class="error"></span>
                </div>
                <div class="add-article-row">
                    <span id="updateStoryMessage" class="message"></span>
                </div>
                <div class="add-article-row">                    
                    <input id="btnUpdateStory" type="submit" class="button" value="Zapisz zmiany"/>
                </div>
            </div>
        </form>
    </div>
</div>