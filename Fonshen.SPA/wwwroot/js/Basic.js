/* ---- Page Class ---- */
var Page = {
    Load: function (path) {
        if (!path) path = location.pathname;
        if (path == "/") path = "/Home/Index";
        var info = path.split("/");
        var script = "/js/" + info[1] + "/" + info[2] + ".js";
        $.ajax({
            url: script, dataType: 'script', cache: true, success: function () {
                Page.Render();
            }
        });
    },
    Jump: function (path, no_history) {
        $.getJSON(path, { _by_ajax: 1 }, function (data) {
            if (!no_history && path != Page.Path) window.history.pushState(null, null, path);
            Page.Path = path;
            Page.Data = data;
            Page.Load(path);
        });
    }
};

$(function () {
    window.addEventListener("popstate", function (e) {
        Page.Jump(location.pathname, true);
    });
    Page.Body = $("#content");
    Page.Load();
});