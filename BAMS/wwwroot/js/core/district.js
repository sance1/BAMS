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
        Swal.fire('Failed!', layoutText.District_popup_txt_invalid_format_file , 'error')
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
            Swal.fire('Failed!', layoutText.District_popup_txt_invalid_format_file , 'error')
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
    var url = '/District/UploadDistricts';
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
                    title: layoutText.District_popup_txt_district_uploaded_successfully
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


function openModalImport() {
    $("#modal-import").modal("show");
}

function allocate(obj, id) {
    var row = $(tbl).DataTable().rows().data().toArray().find(a => a.Uid == id);
    $("#ACdistrict").val(row.Name);
    $("#districtUid").val(row.Uid);
    $("#qty").val(row.Students);
    $("#modal-allocate").modal("show");
}

function go_allocate() {
    let uid = $("#districtUid").val();
    let qty = parseInt($("#qty").val());

    if (qty <= 0) {
        Swal.fire({
            title: 'Error',
            text: layoutText.District_popup_txt_quantity_must_be_more_than,
            icon: 'error'
        });
        return;
    }

    let url = `/district/allocatecode?districtUid=${uid}&qty=${qty}`;

    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        success: function (response) {
            if (response.status == 0) {
                Swal.fire({
                    title: 'Success',
                    text: response.message,
                    icon: 'success'
                }).then((result) => {
                    location.reload();
                });
            }

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


/********************/

const Toast = Swal.mixin({
    toast: true,
    position: 'top',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});

function reloadList() {
    $('#tbl').DataTable().ajax.reload();
}

function initEditor() {
    $("#uid").val(0);
    if ($("#projectUid").is(":visible")){
        $("#projectUid").val(0);
    }
    
    $("#name").val('');
    $("#picName").val('');
    $("#remarks").val('');
}

function showEditor() {
    $('#district-editor').modal('show');
}

function hideEditor() {
    $("#district-editor").modal("hide");
    initEditor();
}

function setEditorTitle(title) {
    $('#district-editor .modal-title').text(title);
}

function newDistrict(projectUid=0) {
    initEditor();
    setEditorTitle("Create New District");
    if(projectUid > 0) $('#projectUid').val(projectUid);
    showEditor();
}

function editDistrict(uid) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `/get-district/${uid}`,
        dataType: "json",
        success: function (response) {
            if (response.status != 'OK') {
                if (response.message == Null) {
                    response.message = "Failed";
                }
                Swal.fire({
                    title: 'Success',
                    text: response.message,
                    icon: 'Success'
                });
                return;
            }

            let district = response.district;
            setEditorTitle("Edit District");
            $('#uid').val(district.uid);
            $('#projectUid').val(district.projectUid);
            $('#name').val(district.name);
            $('#picName').val(district.picName);
            $('#remarks').val(district.remarks);

            showEditor();
        },
        error: function (response) {
            console.log(response)
            if (response.message == Null) {
                response.message = "Edit District Function Error";
            }
            Swal.fire({
                title: 'Error',
                text: response.responseJSON.message,
                icon: 'error'
            });
        }
    });
}

function deleteDistrict(uid) {
    Swal.fire({
        title: 'Delete Confirmation',
        text: layoutText.District_popup_txt_you_sure_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/delete-district/${uid}`,
                dataType: "json",
                success: function (response) {
                    if (response.status == 'OK') {
                        reloadList();
                        Toast.fire({
                            icon: 'success',
                            title: layoutText.District_popup_txt_deleted_successfully
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

function saveDistrict() {

    let uid = parseInt($("#uid").val());

    let district = {
        ProjectUid: parseInt($("#projectUid").val()),
        Name: $("#name").val(),
        PIC: $("#picName").val(),
        Remarks: $("#remarks").val(),
    };
    if (uid > 0) district.uid = uid;

    let validationResult = validateInput(district)
    if (validationResult.length > 0) {
        Swal.fire({
            title: 'Data Validation',
            html: validationResult.join('<br>'),
            icon: 'error'
        });
        return;
    }

    let url = uid == 0
        ? '/district/create'
        : '/district/update';

    sendRequest(url, JSON.stringify(district));
}

$('#modal-district').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.uid;
    if (id) location.href = '/district'
});

function validateInput(input) {
    let err = [];
    if (input.name == "") {
        err.push(layoutText.District_popup_txt_district_name_cannot_empty);
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
            if (response.status == "OK") {
                reloadList();
                hideEditor();
                Toast.fire({
                    icon: 'success',
                    title: layoutText.District_popup_txt_saved_successfully
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
            Swal.close();
            Swal.fire({
                title: 'Error',
                text: response.responseJSON.message,
                icon: 'error'
            });
        }
    });
}