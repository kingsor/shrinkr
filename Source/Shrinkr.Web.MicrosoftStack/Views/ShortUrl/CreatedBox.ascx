﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CreateUrlViewModel>" %>
<div class="messageBox" style="display:<%= (Model == null) ? "none" : string.Empty %>">
    <% if (Model != null) {%>
        <span>Your new urls are generated successfully.</span>
        <ul>
            <li>
                <div>Your shrinked urls are:</div>
                <div><label for="newUrl">New:</label><input id="newUrl" type="text" value="<%= Model.VisitUrl %>" readonly="readonly" class="largeTextBox readOnlyTextBox"/> (<%= Model.VisitUrl.Length %> characters) <a href="<%= Model.VisitUrl %>" target="_blank">[opens in new window]</a></div>
                <div><label for="previewUrl">Preview:</label><input id="previewUrl" type="text" value="<%= Model.PreviewUrl %>" readonly="readonly" class="largeTextBox readOnlyTextBox"/> <a href="<%= Model.PreviewUrl %>" target="_blank">[opens in new window]</a></div>
            </li>
            <li>Your original url was <%= Model.Url %> (<%= Model.Url.Length %> characters) <a href="<%= Model.Url %>" target="_blank">[opens in new window]</a>.</li>
            <li>We have made your url <strong><%= Model.ReducedPercent.ToString("0.#", Culture.Current) %>%</strong> (<strong><%= Model.ReducedCharacters %></strong> characters) <strong><%= Model.HasReduced ? "shorter" : "larger" %></strong>.</li>
        </ul>
    <% } %>
</div>