

//BlockUI functions
function BlockUi(selector, blockMessage) {
    //debugger;
    //
    //if (blockMessage == "" || blockMessage == undefined) {
    //style = 'background-color: #00519c;'
    //blockMessage = "<div ><img src='/Content/images/loaderImg3.gif'></div>";
    blockMessage = "<div><img src='data:image/gif;base64,R0lGODlhEAALAPQAAP////////7+/v7+/v7+/v7+/v////7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/v7+/gAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCwAAACwAAAAAEAALAAAFLSAgjmRpnqSgCuLKAq5AEIM4zDVw03ve27ifDgfkEYe04kDIDC5zrtYKRa2WQgAh+QQJCwAAACwAAAAAEAALAAAFJGBhGAVgnqhpHIeRvsDawqns0qeN5+y967tYLyicBYE7EYkYAgAh+QQJCwAAACwAAAAAEAALAAAFNiAgjothLOOIJAkiGgxjpGKiKMkbz7SN6zIawJcDwIK9W/HISxGBzdHTuBNOmcJVCyoUlk7CEAAh+QQJCwAAACwAAAAAEAALAAAFNSAgjqQIRRFUAo3jNGIkSdHqPI8Tz3V55zuaDacDyIQ+YrBH+hWPzJFzOQQaeavWi7oqnVIhACH5BAkLAAAALAAAAAAQAAsAAAUyICCOZGme1rJY5kRRk7hI0mJSVUXJtF3iOl7tltsBZsNfUegjAY3I5sgFY55KqdX1GgIAIfkECQsAAAAsAAAAABAACwAABTcgII5kaZ4kcV2EqLJipmnZhWGXaOOitm2aXQ4g7P2Ct2ER4AMul00kj5g0Al8tADY2y6C+4FIIACH5BAkLAAAALAAAAAAQAAsAAAUvICCOZGme5ERRk6iy7qpyHCVStA3gNa/7txxwlwv2isSacYUc+l4tADQGQ1mvpBAAIfkECQsAAAAsAAAAABAACwAABS8gII5kaZ7kRFGTqLLuqnIcJVK0DeA1r/u3HHCXC/aKxJpxhRz6Xi0ANAZDWa+kEAA7AAAAAAAAAAAA'></div>";
    //}

    setTimeout(function () {
        if ((typeof selector) == "string") {
            $(selector).block({
                css: { backgroundColor: null, border: null },

                message: blockMessage
            });
        } else {
            selector.block({
                css: { backgroundColor: null, border: null },
                message: blockMessage
            });
        }
    }, 10);
}
function UnBlockUi(selector) {
    if ((typeof selector) == "string") {
        $(selector).unblock();
    } else {
        selector.unblock();
    }
}
//Ajax functions
function Ajax(url, httpMethod, data, isAysnc, beforeSendFunction, successFunction, errorFunction, completeFunction) {
    $.ajax({
        url: url,
        type: httpMethod,
        data: data,
        async: isAysnc,
        beforeSend: beforeSendFunction,
        success: new function () {
            successFunction();
        },
        error: errorFunction,
        complete: completeFunction
    });
}
//Ajax functions
var ajaxOptionsDefault =
{
    IsAysnc: true,
    Cache: true,
    BeforeSendFunction: null,
    CompleteFunction: null
};
function AjaxGet(url, myData, successFunction, errorFunction, ajaxOptions) {
    var options = {};
    if (ajaxOptions != null) {
        options = $.extend(ajaxOptionsDefault, ajaxOptions);
    }    
    //console.log(url + " ---- " + options.IsAysnc);
    $.ajax({
        url: url,
        type: "GET",
        data: myData != "" ? myData : {},
        async: (options != null && options.IsAysnc != "") ? options.IsAysnc : true,
        cache: (options != null && options.Cache != "") ? options.Cache : true,
        beforeSend: function () {
            if (options != null && options.BeforeSendFunction != null && $.isFunction(options.BeforeSendFunction)) {
                options.BeforeSendFunction();
            }
        },
        success: function (data, textStatus, jqXhr) {
            if (successFunction != null && $.isFunction(successFunction)) {
                successFunction(data, textStatus, jqXhr);
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            if (errorFunction != null && $.isFunction(errorFunction)) {
                errorFunction(jqXhr, textStatus, errorThrown);
            }
        },
        complete: function () {
            if (options != null && options.CompleteFunction != null && $.isFunction(options.CompleteFunction)) {
                options.CompleteFunction();
            }
        }
    });
}
function AjaxPost(url, data, successFunction, errorFunction, ajaxOptions) {
    $.ajax({
        url: url,
        type: "POST",
        data: data != "" ? data : {},
        async: ajaxOptions.IsAysnc != "" ? ajaxOptions.IsAysnc : true,
        cache: ajaxOptions.Cache,
        beforeSend: function () {
            if (ajaxOptions.BeforeSendFunction != null && $.isFunction(ajaxOptions.BeforeSendFunction)) {
                ajaxOptions.BeforeSendFunction();
            }
        },
        success: function () {
            if (successFunction != null && $.isFunction(successFunction)) {
                successFunction();
            }
        },
        error: function () {
            if (errorFunction != null && $.isFunction(errorFunction)) {
                errorFunction();
            }
        },
        complete: function () {
            if (ajaxOptions.CompleteFunction != null && $.isFunction(ajaxOptions.CompleteFunction)) {
                ajaxOptions.CompleteFunction();
            }
        }
    });
}

//FillDropDown
function FillDropdown(selector, array, valuekey, textKey) {
    var ddObj = $(selector);
    ddObj.html("<option value=''>--Select--</option>");
    ddObj.next(".holder").html("--Select--");
    if (array.length > 0) {
        $.each(array, function (index, value) {
            ddObj.append($("<option></option>").val(value[valuekey]).text(value[textKey]));
        });
    }
}

//to hide and show body scroll on popup Start 





$(function () {
    $('<img />').attr('src', '~/Content/images/loaderImg3.gif');

    $(".field-error-box").each(function () {
        $(this).css('width', ($(this).prev().width()) + 'px');
    });

    $(".select-wrapper select").focus(function () {
        $(this).closest(".select-wrapper").addClass('select-focused');
    });

    $(".select-wrapper select").blur(function () {
        $(this).closest(".select-wrapper").removeClass('select-focused');
    });

    //$(".disable").disable();

    setTimeout(function () {
        $("[default-focus]").focus();
    }, 10);

    //alertify.set({ buttonReverse: true }); // true, false

    $(".navigateBack").click(function () {
        debugger;
        var elem = $(this);

        
        //alertify.confirm("You'll lose unsaved changes; are you sure you want to navigate away from this page?", function (e) {
            if (e) {

                var URL = $(elem).attr("data-href");

                window.location.assign(URL)
                //  window.location = '@Url.Action("List","SocialMedia")'
                // return true;
            }
            else {
                // return false;
            }

        });
        //return false;
    });




//Function to drag & drop jqgrid row.
function fnDragAndDrogJQGridRow(gridId) {
    $("#" + gridId).sortableRows(); //for row drag & drop.    
    $("tbody:first", "#" + gridId).enableSelection();
}


//Blocks the element from user's interaction
function blockElement(ele) {

    var $coverDiv = $("<div></div>");

    $coverDiv.addClass('blockCover');
    $coverDiv.attr("style", "height: 100%; left: 0px; opacity: 0.1; position: absolute; top: 0px; width: 100%; z-index: 1000; cursor: not-allowed; display: block; background-color: white;");

    $(ele).attr('data-last-position', $(ele).css("position"));
    $(ele).css('position', 'relative');
    $(ele).addClass('blocked');
    $(ele).append($coverDiv);

}

//Unblocks the element from user's interaction
function unblockElement(ele) {
    $(ele).removeAttr("data-last-position");
    $(ele).find(".blockCover").remove();
    $(ele).removeClass('blocked');
}


(function ($) {
    $.BootstrapPopUp = function (options) {
        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
        }, options);

        $(settings.ModalId).modal('show');
        $(settings.ModalTitleID).html(settings.ModalTitle)
        var loaderStr = "<div class='text-center'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span></div>";
        $(settings.ModalBodyId).html(loaderStr);

        AjaxGet(settings.MyHref, {}, function (data, textStatus, jqXhr) {
            $(settings.ModalBodyId).html(data);
        }, settings.LoadWidgetListErrorCallback, null);

    };
}(jQuery));