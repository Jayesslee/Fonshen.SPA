/* ---- Page Class ---- */
var Page = {
    Load: function () {
        Page.Data = Page.Raw.data;
        Page.LoadCss();
        $.ajax({
            url: Page.Raw.js, dataType: 'script', cache: true, success: function () {
                Page.Render();
            }
        });
    },
    Jump: function (path, no_history) {
        $.getJSON(path, { _by_ajax: 1 }, function (raw) {
            if (!no_history && path != Page.Path) window.history.pushState(null, null, path);
            Page.Path = path;
            Page.Raw = raw;
            Page.Load();
        });
    },
    LoadCss: function () {
        var tag = document.getElementById("_load_css");
        var head = document.getElementsByTagName("head").item(0);
        if (tag) head.removeChild(tag);
        var link = document.createElement("link");
        link.href = Page.Raw.css;
        link.rel = "stylesheet";
        link.type = "text/css";
        link.id = "_load_css";
        head.appendChild(link);
    }
};

$(function () {
    window.addEventListener("popstate", function (e) {
        Page.Jump(location.pathname, true);
    });
    Page.Body = $("#content");
    Page.Load();
});