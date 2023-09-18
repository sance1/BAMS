var fileobj;
var rawXls;
$("#drop_zone").on("dragover", function (event) {
    event.preventDefault();
    event.stopPropagation();
    return false;
});
$("#drop_zone").on("drop", function (event) {
    event.preventDefault();
    event.stopPropagation();
    var tmp = event.originalEvent.dataTransfer.files[0];
    if (tmp.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
        Swal.fire('Failed!', layoutText.invalidFormatFile, 'error')
        return;
    }
    fileobj = event.originalEvent.dataTransfer.files[0];
    var fname = fileobj.name;
    var fsize = fileobj.size;
    if (fname.length > 0) {
        document.getElementById('file_info').innerHTML = "File name : " + fname + ' <br>File size : ' + bytesToSize(fsize);
    }
    document.getElementById('selectfile').files[0] = fileobj;
    // document.getElementById('btn_upload').style.display="inline";
});
$('#btn_file_pick').click(function () {
    /*normal file pick*/
    document.getElementById('selectfile').click();
    document.getElementById('selectfile').onchange = function () {
        var tmp = document.getElementById('selectfile').files[0];
        if (tmp.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
            Swal.fire('Failed!', layoutText.invalidFormatFile, 'error')
            return;
        }
        fileobj = document.getElementById('selectfile').files[0];
        var fname = fileobj.name;
        var fsize = fileobj.size;
        if (fname.length > 0) {
            document.getElementById('file_info').innerHTML = "File name : " + fname + ' <br>File size : ' + bytesToSize(fsize);
        }
        // document.getElementById('btn_upload').style.display="inline";
    };
});

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return '0 Byte';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
}

function uploadProcess() {
    if (fileobj) {
        let fileReader = new FileReader();
        fileReader.readAsBinaryString(fileobj);
        fileReader.onload = (event) => {
            let data = event.target.result;
            let workbook = XLSX.read(data, {type: "binary"});
            workbook.SheetNames.forEach(sheet => {
                rawXls = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
            });
            uploadDistrict()
        }
    }
}

function uploadDistrict() {
    var url = '/AdministrativeUnit/UploadAdministrativeUnit';
    var projectId = $("#projectId").val();
    Swal.showLoading()

    $.ajax({
        type: 'POST',
        url: url,
        data: {
            rawXls: JSON.stringify(rawXls),
            projectId
        },
        datatype: "json",
        traditional: true,
        success: function (response) {
            if (response.status === 0) {
                Toast.fire({
                    icon: 'success',
                    title: layoutText.theDistrictListHasBeenUploadedSuccessfully
                });
                setTimeout(function(){
                    location.reload()
                }, 1000)
                return;
            } else {
                Swal.fire('Failed', response.message, 'error')
            }

        },
        error: function (response) {
            if (response.message == null) {
                response.message = "Failed";
            }
            Swal.fire('Failed!', response.message, 'error')
        }
    });
}

const Toast = Swal.mixin({
    toast: true,
    position: 'top',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});


$(document).ready(function () {

    initDataTable()

    $('#projectUid').change(loadAdministrativeUnitList);
    $('#parentUid').click(function () {

        if ($('#projectUid').val() == null) {
            Swal.fire({
                title: 'Info',
                text: 'Please select a project first',
                icon: 'info'
            });
        }
    })

    bindButtons();

});

// ***** DataTable Section *****
function initDataTable() {
    $.getJSON('/account/GetMenuConfig?menu=District', dynamicColumns);

}

function dynamicColumns(values) {
    var mapping = values.map(val => {
        let isNoSrc = /no-search/g.exec(val.Class);
        $(".thead").append(`<th class="${val.Class}">${val.Name}</th>`);
        let noSrc = isNoSrc == null ? "" : "no-search";
        $(".tfoot").append(`<th ${noSrc} class="${val.Class}">${val.Name}</th>`);
        switch (val.Key) {
            case "Id":
                return {
                    data: val.Key, name: val.Name, autoWidth: true, visible: false
                };
            case "No":
                return {
                    data: null, name: "No", autoWidth: true, orderable: false,
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                };
            default:
                return {
                    data: val.Key, name: val.Name, autoWidth: true
                }
        }
    });
    $(".thead").append(`<th>${layoutText.DistrictTblHeadActions}</th>`);
    $(".tfoot").append(`<th no-search>${layoutText.DistrictTblHeadFoot}</th>`);

    $('#administrativeUnitList').DataTable({
        autoWidth: true,
        processing: true,
        dom: 'lrtp',
        filter: true,
        ordering: true,
        serverSide: true,
        ajax: getDataSource(),
        columns: [
            ...mapping,
            {name: 'ActionButtons', data: null, orderable: false, render: getActionButtons, width: "100px"},
            {name: 'Uid', data: 'Uid', visible: false}
        ],
        columnDefs: [
            {targets: '_all', className: 'dt-head-center'}
        ],
        initComplete: onTableInitComplete
    });
}

function getDataSource() {
    return {
        url: '/AdministrativeUnit/GetList',
        type: 'POST',
        datatype: 'json'
    }
}

function getActionButtons(data, type, row, meta) {
    let buttons = '';
    if (data.IsCodeAvailable) {
        buttons += `<button class="btn-aw org" data-uid="${data.Uid}" onclick="allocate(this, '${data.Uid}')" ><img src="/img/icon/create-contract.png" /> <span class="tooltipText">${layoutText.Administrative_btn_tooltip_allocate_codes}</span></button>`;
    }
    buttons += `
            <button class="btn-aw edit" onclick="editUnit(${data.Uid})" ><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Administrative_btn_tooltip_edit}</span></button>
            <button class="btn-aw delete" data-uid="${data.Uid}" onclick="deleteUnit('${data.Uid}')" ><img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Administrative_btn_tooltip_delete}</span></button>`;
    return buttons;
}

function getRowNumber(data, type, row, meta) {
    return meta.row + meta.settings._iDisplayStart + 1;
}

function onTableInitComplete() {
    $('#administrativeUnitList tr.search th').each(function (i) {
        if ($(this).is('[no-search]')) {
            $(this).html('');
            return;
        }
        var title = $(this).text();
        $(this).html('<input type="text" class="Search' + title + '" placeholder="Search" />');
    });
    this.api().columns().every(function () {
        var that = this;
        if (this.footer() != null) {
            $('input', this.footer()).on('keyup change clear', delay(function () {
                if (that.search() !== this.value) {
                    that.search(this.value).draw();
                }
            }));
        }
    });

    //move from tfoot to thead
    $('#administrativeUnitList tr.search').appendTo('#administrativeUnitList thead');
}

// ***** End of DataTable Section *****


function loadAdministrativeUnitList() {
    initParentList();
    let projectUid = parseInt($('#projectUid').val());
    $.ajax({
        type: 'GET',
        url: `/AdministrativeUnit/GetByProject/${projectUid}`,
        success: function (response) {
            if (response.status === 'OK') {
                let $list = $("#parentUid");
                $.each(response.administrativeUnits, function () {
                    $list.append($("<option />").val(this.uid).text(this.name));
                });
                let uid = $list.data('uid');
                if (uid > 0) {
                    $list.val(uid);
                }
            }
        }
    });
}


function bindButtons() {
    $('.x-new-button').click(newUnit);
    $('#unit-editor .x-save-button').click(saveUnit);
    $('#unit-editor .x-cancel-button').click(hideEditor);
}

function reloadList() {
    $('#administrativeUnitList').DataTable().ajax.reload(null, false);
}

function initEditor() {
    $("#uid").val(0);
    $("#projectUid").val(null);
    initParentList();
    $("#name").val('');
    $("#picName").val('');
    $("#remarks").val('');
}

function initParentList() {
    let $list = $("#parentUid");
    $list.html('');
    $list.data('uid', 0);
    $list.append($("<option />").val(0).text(''));
}


function showEditor() {
    $('#unit-editor').modal('show');
}

function hideEditor() {
    $("#unit-editor").modal("hide");
    initEditor();
}

function setEditorTitle(title) {
    $('#unit-editor .modal-title').text(title);
}

function newUnit() {
    initEditor();
    setEditorTitle("New Administrative Unit");
    showEditor();
}


function editUnit(uid) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `/get-administrative-unit/${uid}`,
        dataType: "json",
        success: function (response) {
            if (response.status != 'OK') {
                if (response.message == null) {
                    response.message = "Failed";
                }
                Swal.fire({
                    title: 'Error',
                    text: response.message,
                    icon: 'error'
                });
                return;
            }

            let unit = response.administrativeUnit;
            setEditorTitle("Edit Administrative Unit");
            $('#uid').val(unit.uid);
            $('#projectUid').val(unit.projectUid);
            loadAdministrativeUnitList();
            $('#parentUid').data('uid', unit.parentUid);
            $('#name').val(unit.name);
            $('#picName').val(unit.picName);
            $('#remarks').val(unit.remarks);

            showEditor();
        },
        error: function (response) {
            if (response.message == null) {
                response.message = "Edit School Function Error";
            }
            Swal.fire({
                title: 'Error',
                text: response.message,
                icon: 'error'
            });
        }
    });
}

function openModalImport() {
    $("#modal-import").modal("show");
}

function saveUnit() {

    let uid = parseInt($("#uid").val());

    let administrativeUnit = {
        uid: uid,
        projectUid: parseInt($("#projectUid").val()),
        parentUid: parseInt($("#parentUid").val()),
        name: $("#name").val(),
        pic: $("#picName").val(),
        remarks: $("#remarks").val(),
    };

    let validationResult = validateInput(administrativeUnit)
    if (validationResult.length > 0) {
        Swal.fire({
            title: 'Data Validation',
            html: validationResult.join('<br>'),
            icon: 'error'
        });
        return;
    }

    let url = uid == 0
        ? '/AdministrativeUnit/Create'
        : '/AdministrativeUnit/Update';

    sendRequest(url, JSON.stringify(administrativeUnit));
}


function validateInput(input) {
    let err = [];
    if (isNaN(input.projectUid)) {
        err.push("Please select a project");
    }
    if (input.name == "") {
        err.push("Name cannot empty");
    }
    if (input.PIC == "") {
        err.push("PIC cannot empty");
    }
    if (input.Students <= 0) {
        err.push("Students cannot empty");
    }
    return err;
}

function sendRequest(url, payload) {
    Swal.showLoading();

    $.ajax({
        type: "POST",
        url: url,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: payload,
        success: function (response) {
            Swal.close();
            if (response.status == 'OK') {
                reloadList();
                hideEditor();
                Toast.fire({
                    icon: 'success',
                    title: 'The administrative unit data has been saved successfully.'
                });
                return;
            }
            Swal.fire({
                title: 'Error',
                text: response.message,
                icon: 'error'
            });
        },
        error: function (response) {
            Swal.fire({
                title: 'Error',
                text: response.responseJSON.message,
                icon: 'error'
            });
        }
    });
}

function deleteUnit(uid) {
    Swal.fire({
        title: 'Delete Confirmation',
        text: 'Are you sure you want to delete the administrative unit?',
        icon: 'warning',
        showDenyButton: true,
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/delete-administrative-unit/${uid}`,
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.status == 'OK') {
                        reloadList();
                        Toast.fire({
                            icon: 'success',
                            title: 'Administrative unit has been deleted successfully'
                        })
                        return;
                    }
                    Swal.fire({
                        title: 'Failed',
                        text: response.message,
                        icon: 'success'
                    });
                },
                error: function (response) {
                    if (response.message == null) layoutText.failed;
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                }
            });
        }
    });
}