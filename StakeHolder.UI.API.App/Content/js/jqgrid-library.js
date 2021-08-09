
myDelOptions = {
    // because I use "local" data I don't want to send the changes to the server
    // so I use "processing:true" setting and delete the row manually in onclickSubmit
    onclickSubmit: function (rp_ge, rowid) {
        // we can use onclickSubmit function as "onclick" on "Delete" button
        //alert("The row with rowid=" + rowid + " will be deleted");

        // delete row
        $('#' + GridJq.GridId).delRowData(rowid);
        $("#delmod" + GridJq.GridId).hide();
        //$('div.ui-widget-overlay').hide();

        if ($('#' + GridJq.GridId)[0].p.lastpage > 1) {
            // reload grid to make the row from the next page visable.
            // TODO: deleting the last row from the last page which number is higher as 1
            //$("#" + GridJq.GridId).trigger("reloadGrid", [{ page: $('#' + GridJq.GridId)[0].p.page }]);
        }

        return true;
    },
    processing: true
};

lastCol = 0;
var colHeaderAlignment = [];
var grid;
function columnsData(colData) {

    var str = "[";
    str = str;

    for (var i = 0; i < colData.Columns.length; i++) {

        str = str + "{name:'" + colData.Columns[i].DBName + "', sortable: " + colData.Columns[i].Sortable + ", index: '" + colData.Columns[i].Index + "', width: '" + colData.Columns[i].Width + "',  hidden: " + colData.Columns[i].Hidden + ",  resizable: false";

        if (colData.Columns[i].align != "" && colData.Columns[i].align != undefined) {
            str = str + ", align: '" + colData.Columns[i].align + "'";
        }

        if (colData.Columns[i].Formatter != "" && colData.Columns[i].Formatter != undefined) {
            if (colData.Columns[i].Customfunction) {
                str = str + ", formatter: " + colData.Columns[i].Formatter;
            }
            else {
                str = str + ", formatter: '" + colData.Columns[i].Formatter + "'";
            }
        }


        if (colData.Columns[i].Formatoptions != "" && colData.Columns[i].Formatoptions != undefined) {

            str = str + ", formatoptions: " + JSON.stringify(colData.Columns[i].Formatoptions);
        }

        if (colData.Columns[i].HeaderAlignment != "" && colData.Columns[i].HeaderAlignment != undefined) {
            colHeaderAlignment.push({ ID: "div#jqgh_" + GridJq.GridId + "_" + colData.Columns[i].DBName, Alignment: colData.Columns[i].HeaderAlignment });
        }

        if (colData.Columns[i].CellAttr != "" && colData.Columns[i].CellAttr != undefined) {
            str = str + ", cellattr : " + colData.Columns[i].CellAttr;
        }

        if (colData.Columns[i].Editable == true) {
            str = str + ", editable: " + colData.Columns[i].Editable;
            if (colData.Columns[i].EditType != "" && colData.Columns[i].EditType != undefined) {
                str += ", edittype: '" + colData.Columns[i].EditType + "'";
            }
            if (colData.Columns[i].editoptions.value != "" && colData.Columns[i].editoptions.value != undefined) {
                str += ", editoptions: '" + colData.Columns[i].editoptions + "'";

            }
        }

        //if (Data[i].trim() == 'SOURCEKEYID') {
        //    str = str + ",editable: true, editrules: { edithidden: true }, hidden: true ";
        //}

        if (i < colData.Columns.length - 1)
            str = str + "},";
        else
            str = str + "}";

        lastCol = i;
    }
    //if (datajsonrow != '') {
    //    str = str + datajsonrow;
    //}

    if (colData.IsEditable) {
        str += ", {name:'IsEdited',index:'IsEdited', hidden: true,align:'center',sortable:false}";
        lastCol += 1;
    }

    str = str + "]";
    // alert(str+"mystr");
    return str;
}

function columnsName(colData) {
    var str = "[";
    str = str;

    for (var i = 0; i < colData.Columns.length; i++) {

        str += "'" + colData.Columns[i].Name + "'";

        if (i < colData.Columns.length - 1)
            str += ",";

    }

    if (colData.IsEditable) {
        str += ", 'IsEdited'";
    }
    str = str + "]";
    return str;
}
var GridJq;
function InitGrid() {
    GridJq = {
        GridId: '#_visitorLogGrid',
        Url: "", SearchURL: "", SearchString: "", RowNum: 10, RowNumbers: false, RowList: [], Pager: '#_visitorLogPager', SortName: 'ID', ViewRecords: true, SortOrder: "desc", LoadOnce: false, Datatype: "json", LocalData: "", EditUrl: "", Caption: "", Width: 1000, Height: 200, HideGrid: false, IsEditable: false, Columns: [], addCol: function (col) {
            this.Columns.push(col);
        }
    };
}
var Col = { Name: "ID", DBName: "ID", Width: 1, Index: "ID", Hidden: false, Key: false, Editable: false, Sortable: true, Formatter: "", EditType: "", editoptions: { value: "" }, align: "Left" };



var lastsel2;

function saverow(rowid, oldrow, newrow) {
    $('#' + GridJq.GridId).jqGrid('saveRow', rowid,
        {
            successfunc: function (response) {
                //$("#" + rowid).find('td').eq('' + lastCol + '').html('Y');
                $("#" + GridJq.GridId + " tr").eq(rowid).children().eq(lastCol).html('Y');
                //updateEditFlag(rowid);setTimeout
                updateButtons(rowid, oldrow, newrow, true);
            }
        });

}



function ReloadGrid(response) {
    var grid = jQuery("#" + GridJq.GridId);
    grid.clearGridData();
    if (searchTerm != null) {
        grid.setGridParam({ url: GridJq.SearchURL + GridJq.SearchString });
    }
    else {
        grid.setGridParam({ url: GridJq.Url });
    }
    grid.setGridParam({ datatype: GridJq.Datatype });
    grid.trigger('reloadGrid', [{ current: true }]);
    // setTimeout('reload(' + currentPageVar + ')', 100);
}

function restorerow(oldrow, newrow) {
    updateButtons(lastsel2, oldrow, newrow, true);
    //$("#" + GridJq.GridId + " tr").eq(lastsel2).children().eq(oldrow).css('display', 'block');
    //$("#" + GridJq.GridId + " tr").eq(lastsel2).children().eq(newrow).css('display', 'none');
    $('#' + this.GridJq.GridId).restoreRow(lastsel2);
}

function editrow(data, oldrow, newrow) {
    var id = data;
    //$("#" + GridJq.GridId + " tr").eq(lastsel2).children().eq(oldrow).css('display', 'block');
    //$("#" + GridJq.GridId + " tr").eq(lastsel2).children().eq(newrow).css('display', 'none');
    updateButtons(lastsel2, oldrow, newrow, true)
    if (id && this.GridJq.IsEditable) {
        $('#' + this.GridJq.GridId).restoreRow(lastsel2);
        $('#' + this.GridJq.GridId).editRow(id, true);
        lastsel2 = id;
    }

    updateButtons(data, oldrow, newrow, false);
}


function updateButtons(rowId, oldrow, newrow, activate) {
    if (activate) {
        $("#" + GridJq.GridId + " tr").eq(rowId).children().eq(oldrow).css('display', 'block');
        $("#" + GridJq.GridId + " tr").eq(rowId).children().eq(newrow).css('display', 'none');
    }
    else {
        $("#" + GridJq.GridId + " tr").eq(rowId).children().eq(oldrow).css('display', 'none');
        $("#" + GridJq.GridId + " tr").eq(rowId).children().eq(newrow).css('display', 'block');
    }
}

$(window).on("resize", function () {
    var newWidth = $('#' + GridJq.GridId).closest(".ui-jqgrid").parent().width();
    $('#' + GridJq.GridId).jqGrid("setGridWidth", newWidth, true);
});

$(window).on("load", function () {
    //setTimeout(function () {
    //    var newWidth = $('#' + GridJq.GridId).closest(".ui-jqgrid").parent().width();
    //    $('#' + GridJq.GridId).jqGrid("setGridWidth", newWidth, true);
    //}, 100);
});

function deleterow(data) {
    var id = data;

    // confirm dialog
    alertify.confirm("Do you want to delete the " + data + " record!", function (e) {
        if (e) {
            alert('done');// user clicked "ok"
        } else {
            // user clicked "cancel"
            alert('cancel');
        }
    });
}

function completeload(data) {
}

var nullFormatter = function (cellvalue, options, rowObject) {
    var nullvalue = '';
    if (cellvalue == null || cellvalue === 'NULL') {
        nullvalue = '';
    }
    else {
        nullvalue = cellvalue;
    }
    return nullvalue;
}

function showGrid(GridJqData) {
    grid == $("#" + GridJqData.GridId);
    var coldata = columnsData(GridJqData);
    var colname = columnsName(GridJqData);


    coldata = eval('{' + coldata + '}');
    colname = eval('{' + colname + '}');

    $('#' + GridJqData.GridId).jqGrid({
        caption: GridJqData.Caption,
        colNames: colname,
        colModel: coldata,
        onSelectRow: function (id) {
            //if (id && id !== lastsel2 && GridJqData.IsEditable) {
            //    $('#' + GridJqData.GridId).restoreRow(lastsel2);
            //    $('#' + GridJqData.GridId).editRow(id, true);
            //    lastsel2 = id;
            //}
        },
        // loadonce: function () { return GridJqData.LoadOnce != undefined && GridJqData.LoadOnce != "" ? GridJqData.LoadOnce : false; },
        loadonce: GridJqData.LoadOnce, //added LoadOnce property: True for client side sorting , False for server side sorting
        datatype: GridJqData.Datatype,
        gridview: true,
        ignoreCase: true,
        autoencode: true,
        shrinkToFit: true,
        autowidth: true,
        rownumbers: GridJqData.RowNumbers,
        onCellSelect: function (rowid, icol, cellcontent, e) { },//alert('Cell Selected');},
        hidegrid: GridJqData.HideGrid,
        pager: GridJqData.Pager,
        sortname: GridJqData.SortName,
        rowNum: GridJqData.RowNum,
        rowList: GridJqData.RowList,
        sortorder: GridJqData.SortOrder,
        width: GridJqData.Width,
        height: 'auto',
        viewrecords: false,
        //loadError: function (xhr, status, error) {
        //    alert('Empty');
        //    jQuery('#' + GridJqData.GridId > div.ui - jqgrid - bdiv > div > div).text("No Record Found.");
        //},
        emptyrecords: 'No Record Found.',
        mtype: 'GET',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            userdata: "userdata"
        },
        loadBeforeSend: function () {
            colHeaderAlignment.forEach(function (head) {
                $(head.ID).css("text-align", head.Alignment);
            })

            $("div#jqgh_" + GridJqData.GridId + "_Actions").css("cursor", "default");
            $("div#jqgh_" + GridJqData.GridId + "_Action").css("cursor", "default");
        },
        loadComplete: function (data) {
            var cnt = jQuery("#" + GridJqData.GridId).jqGrid('getGridParam', 'records');

            if (cnt == 0) {
                //jQuery('.ui-jqgrid-hbox' + GridJqData.GridId > div.ui - jqgrid - bdiv > div > div).text("No Record Found.");
                $("#divPager_center").css('visibility', 'hidden');
                if (!$("#dvNoRecFound_" + GridJqData.GridId).text().length > 0)
                    if (GridJqData.EmptyMsg == "" || GridJqData.EmptyMsg == undefined)
                        //$("<div id='dvNoRecFound'><span class='errorText'>No Record Found!!</span></div>").insertBefore("#" + GridJqData.GridId);
                        $("<div id='dvNoRecFound_" + GridJqData.GridId + "' class=''><span>No Record Found.</span></div>").insertBefore("#" + GridJqData.GridId);
                    else
                        //$("<div id='dvNoRecFound'><span class='errorText'>" + GridJqData.EmptyMsg + "</span></div>").insertBefore("#" + GridJqData.GridId);
                        $("<div id='dvNoRecFound_" + GridJqData.GridId + "' class=''><span>" + GridJqData.EmptyMsg + "</span></div>").insertBefore("#" + GridJqData.GridId);
                else
                    $("#dvNoRecFound_" + GridJqData.GridId).show();
            }
            else {
                $("#divPager_center").css('visibility', 'visible');
                if ($("#dvNoRecFound_" + GridJqData.GridId).text().length > 0)
                    $("#dvNoRecFound_" + GridJqData.GridId).hide();
            }

            var newWidth = $('#' + GridJqData.GridId).closest(".ui-jqgrid").parent().width();
            $('#' + GridJqData.GridId).jqGrid("setGridWidth", newWidth, true);

            completeload(data);

        },
        url: GridJq.Url,
        data: GridJq.LocalData,
        gridComplete: function () {
            //alert('complete');


            var ids = jQuery("#" + GridJqData.GridId).jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<input name='act' id=" + ids[i] + "_act role='textbox' style='width:98%;' type='text' value='" + eval(i + 1) + "'   />";
                jQuery("#" + GridJqData.GridId).jqGrid('setRowData', ids[i], { act: be });
            }
        },
        onLoadSuccess: function () { alert('load complete') }
    }).navGrid(GridJqData.Pager, { view: false, del: false, add: false, edit: false, search: false },
{ width: 400 }, // default settings for edit
{}, // default settings for add
{}, // delete instead that del:false we need this
{}, // search options
{} /* view parameters*/
);

};
