const Toast = Swal.mixin({
    toast: true,
    position: 'top',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});


$(document).ready(function () {
    bindButtons();
})


function bindButtons() {
    $('.add-student-button').click(newStudent);
    $('#student-editor .save-button').click(saveStudent);
    $('#student-editor .hide-editor-button').click(hideEditor);
}

function reloadList() {
    $('#user-account-list').DataTable().ajax.reload(null, false);
}

function initEditor() {
    $("#uid").val(0);
    $("#class").val('');
    $("#name").val('');
    $("#username").val('');
    $("#phoneNumber").val('');
    $("#email").val('');
}

function showEditor() {
    $('#student-editor').modal('show');
}

function hideEditor() {
    $("#student-editor").modal("hide");
    initEditor();
}

function setEditorTitle(title) {
    $('#student-editor .modal-title').text(title);
}

function newStudent() {
    initEditor();
    setEditorTitle(layoutText.Student_form_title_txt_new_student);
    showEditor();
}

function editStudent(uid) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `/get-student/${uid}`,
        dataType: "json",
        success: function (response) {
            if (response.status != 'OK') {
                Swal.fire({
                    title: 'Error',
                    text: response.message,
                    icon: 'error'
                });
                return;
            }

            let student = response.student;
            setEditorTitle("Edit Student");
            $('#uid').val(student.uid);
            $('#class').val(student.className);
            $('#name').val(student.name);
            $('#username').val(student.username);
            $('#phoneNumber').val(student.phoneNumber);
            $('#email').val(student.email);

            showEditor();
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

function deleteStudent(uid) {
    Swal.fire({
        title: 'Delete Confirmation',
        text: layoutText.Student_popup_txt_want_to_delete_the_student,
        icon: 'warning',
        showDenyButton: true,
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/delete-student/${uid}`,
                dataType: "json",
                success: function (response) {
                    if (response.message == (null || "")) {
                        response.message = "Failed";
                    }
                    if (response.status == 'OK') {
                        reloadList();
                        Toast.fire({
                            icon: 'success',
                            title: layoutText.Student_toast_txt_deleted_successfully
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
                    if (response.message == null) response.message = "Failed";
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

/*
$("#create-form").submit(function (e) {
    document.getElementById("save").disabled = true;
    e.preventDefault();
    var url = '/student/create'
    var username = $("#username").val();
    var name = $("#name").val();
    var email = $("#email").val();
    var phone = $("#phone").val();
    var uid = $("#id").val();
    if (uid !== "0") {
        url = '/student/update'
    }
    var token = $('input[name="__RequestVerificationToken"]').val();

    var data = {
        "username": username,
        "email": email,
        "name": name,
        "phoneNumber": phone,
        "uid": +uid,
        "class": "",
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            document.getElementById("save").disabled = false;
            if (result.message == (null || "")) {
                result.message = layoutText.Student_popup_txt_failed;
            }
            if (result.status == 0) {
                Swal.fire({
                    title: 'Success',
                    text: "Success",
                    icon: 'success'
                }).then((result) => {
                    // history.back();
                    // location.href = '/userAccount';
                    reloadData()
                });
            } else {
                Swal.fire({
                    title: 'Info',
                    html: result.message,
                    icon: 'info'
                })
            }
        },
        error: function (xhr, resp, text) {
            document.getElementById("save").disabled = false;
            // console.log(xhr, resp, text);
        }
    })
})
*/

function getData() {
    var columnCount = 0;
    var tbl = $('#user-account-list').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: getDataSource(),
        columns: [
            {
                data: null, name: "No", autoWidth: true, orderable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {data: "Id", name: "Id", autoWidth: true, visible: false},
            {data: "Name", name: "Name", autoWidth: true},
            {data: "UserName", name: "Username", autoWidth: true},
            {data: "Class", name: "Class", autoWidth: true},
            {data: "PhoneNumber", name: "PhoneNumber", autoWidth: true},
            {data: "Email", name: "Email", autoWidth: true},
            {
                data: "ActivationStatus", name: "ActivationStatus", autoWidth: true,
                render: function (data, type) {
                    return data ? "Yes" : "No";
                }
            },
            {
                data: 'Uid',
                "render": getActionButton,
            },
        ]
    });
}

function getDataSource() {
    return {
        "url": "/userAccount/GetListUserAccount",
        "type": "POST",
        "datatype": "json"
    }
}

function getActionButton(data, type) {
    return `<button class="btn-aw light_blue" onclick=""><img src="/img/icon/activation-btn.png" /> <span class="tooltipText">${layoutText.Student_btn_tooltip_activated_student}</span></button>
                        <button class="btn-aw edit" onclick="editStudent(${data})"><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Student_btn_tooltip_edit}</span> </button>
                        <button class="btn-aw delete" onclick="deleteStudent(${data})"><img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Student_btn_tooltip_delete}</span></button>`;
}

function onInitTableComplete() {
    $('#user-account-list tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + layoutText.Student_tbl_txt_search + '" />');
        if (title == "Actions" || title == "No") {
            $(this).html("");
        }
    });
    $('#user-account-list tbody tr').each(function () {
        $('td', this).each(function (i) {
            var value = $(this).text();
            if (!isNaN(+value) && i > 0) {
                $(this).addClass('text-right')
            }
        })
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

    var r = $('#user-account-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#user-account-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}


$('#modal-account').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.uid;
    if (id) location.href = '/userAccount'
});

var fileObj;
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
        Swal.fire('Failed!', layoutText.Student_popup_txt_invalid_format, 'error')
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
            Swal.fire('Failed!', layoutText.Student_popup_txt_invalid_format, 'error')
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
            uploadStudent()
        }
    }
}

function uploadStudent() {
    var url = '/UserAccount/UploadStudents';
    var projectId = $("#projectId").val()
    var admUnitId = $("#admUnitId").val()
    var schoolId = $("#schoolId").val()
    Swal.showLoading()

    $.ajax({
        type: 'POST',
        url: url,
        data: {
            rawXls: JSON.stringify(rawXls),
            projectId,
            admUnitId,
            schoolId
        },
        datatype: "json",
        traditional: true,
        success: function (response) {
            if (response.status === 0) {
                Toast.fire({
                    icon: 'success',
                    title: layoutText.Student_toast_txt_successfully_submitted
                });
                setTimeout(function () {
                    location.reload()
                }, 1000)
            } else {
                if (response.message == null) response.message = layoutText.Student_popup_txt_upload_failed;
                Swal.fire('Failed!', response.message, 'error')
            }
        },
        error: function (response) {
        }
    });

}

function openModalImport() {
    $("#modal-import").modal("show");
    $("#selectfile").val('');
    $("#file_info").html('');
}


function saveStudent() {

    let uid = parseInt($("#uid").val());

    let student = {
        uid: uid,
        class: $("#class").val(),
        name: $("#name").val(),
        username: $("#username").val(),
        phoneNumber: $("#phoneNumber").val(),
        email: $("#email").val()
    };

    let validationResult = validateInput(student)
    if (validationResult.length > 0) {
        Swal.fire({
            title: 'Data Validation',
            html: validationResult.join('<br>'),
            icon: 'error'
        });
        return;
    }

    let url = uid == 0
        ? 'student/create'
        : 'student/update';

    sendRequest(url, JSON.stringify(student));
}

function validateInput(input) {
    let results = [];
    if (input.name == "") {
        results.push(layoutText.Student_popup_txt_student_name_cannot_be_empty);
    }
    if (input.username == "") {
        results.push(layoutText.Student_popup_txt_username_cannot_be_empty);
    }

    return results;
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
                    title: layoutText.Student_toast_txt_saved_successfully
                });
                return;
            }
            Swal.fire({
                title: 'Error',
                html: response.message,
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

$("#projectId").change((e) => {
    getDistrict($("#projectId").val());
    getSchool($("#projectId").val(), $("#districtId").val());
});

$("#districtId").change((e) => {
    getSchool($("#projectId").val(), $("#districtId").val());
});

function getDistrict(projectId) {
    $.getJSON(`/account/GetDistrict?projectId=${projectId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#districtId").html(opt);
    });
}

function getSchool(projectId, districtId) {
    $.getJSON(`/account/GetSchool?projectId=${projectId}&districtId=${districtId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#schoolId").html(opt);
    });
}