// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var AjaxFunctions = new function () {
    var notifyInternal = function (message, type) {
        if (type === undefined) {
            type = "error";
        }
        alert(message);
    };

    var processXhr = function (xhr) {
        if (xhr.responseText) {
            if (xhr.getResponseHeader("content-type") === "application/json; charset=utf-8") {
                var responseObject = JSON.parse(xhr.responseText);
                if (responseObject && responseObject.Error) {
                    notifyInternal("Error: " + responseObject.Error.Message);
                    return;
                }
                // Not sure what form the response content has. Use inspector for details.
                notifyInternal("Error");
                return;
            }
        }

        if (xhr.statusText) {
            notifyInternal("Error: " + xhr.statusText);
        }
        else {
            notifyInternal("Error");
        }
    };

    this.notify = function (message, type) {
        notifyInternal(message, type);
    };

    this.ajaxPost = function (url, data, alwaysAction, okText, doneAction) {
        var ajaxDoneMessage = !okText ? "OK" : okText;
        $.ajax(
            {
                url: url,
                method: "post",
                data: data
            })
            .done(
                function () {
                    notifyInternal(ajaxDoneMessage, "success");
                    if (doneAction) {
                        doneAction();
                    }
                })
            .fail(
                function (xhr) {
                    processXhr(xhr);
                })
            .always(
                function (result) {
                    if (alwaysAction) {
                        alwaysAction(result);
                    }
                });
    };
};