
$("#login-form").submit(function (e) {
    e.preventDefault();
    var username = $("#username").val();
    var password = $("#password").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (!password && !username) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Login_popup_txt_enter_user_and_pass,
            icon: 'error'
        });
        return;
    }

    if (!password) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Login_popup_txt_enter_password,
            icon: 'error'
        });
        return;
    }

    if (!username) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Login_popup_txt_enter_your_username,
            icon: 'error'
        });
        return;
    }

    var data = {
        "username": username,
        "password": password,
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: '/account/login',
        type: "POST",
        data: data,
        success: function (result) {
            if (result == 'ok') {
                location.href = '/'
            } else {
                Swal.fire({
                    title: 'Login Failed',
                    text: layoutText.Login_popup_txt_user_and_pass_not_correct,
                    icon: 'error'
                });
            }
        },
        error: function (xhr, resp, text) {
            console.log(xhr, resp, text);
        }
    })
});

$("#change-password-form").submit(function (e) {
    e.preventDefault();
    var password = $("#password").val();
    var confirmPassword = $("#confirm-password").val();
    var uid = $("#uid").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (!password || !confirmPassword) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Login_popup_txt_password_cannot_be_empty,
            icon: 'error'
        });
        return;
    }

    if (password != confirmPassword) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Login_popup_txt_password_not_same,
            icon: 'error'
        });
        return;
    }

    var data = {
        "password": password,
        "confirmPassword": confirmPassword,
        "uid": uid,
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: '/account/ChangePassword',
        type: "POST",
        data: data,
        success: function (result) {
            if (result == 'ok') {
                Swal.fire({
                    title: 'Success',
                    text: "Success",
                    icon: 'success'
                }).then((result) => {
                    history.back();
                    location.href = '/';
                });
                return;
            }
            Swal.fire({
                title: 'Error',
                text: result,
                icon: 'error'
            });
        },
        error: function (xhr, resp, text) {
            console.log(xhr, resp, text);
        }
    })
})
$("#create-form").submit(function (e) {
    document.getElementById("save").disabled = true;
    e.preventDefault();
    var username = $("#username").val();
    var email = $("#email").val();
    var projectId = $("#project-id").val();
    var contractId = $("#contract-id").val();
    var districtId = $("#district-id").val();
    var admUnitId = $("#admUnit-id").val();
    var schoolId = $("#school-id").val();
    var roleId = $("#role-id").val();
    var organization = $("#organization").val();
    var id = $("#id").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    var getUsername = cekUsername();
    if (getUsername != "") {
        Swal.fire({
            title: 'Error',
            text: getUsername,
            icon: 'error'
        }).then((result) => {
            document.getElementById("save").disabled = false;
        });
        return;
    }
    if (!username) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Account_popup_txt_username_cannot_be_empty,
            icon: 'error'
        }).then((result) => {
            document.getElementById("save").disabled = false;
        });
        return;
    }

    if (!email) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Account_popup_txt_email_cannot_be_empty,
            icon: 'error'
        }).then((result) => {
            document.getElementById("save").disabled = false;
        });
        return;
    }
    if (!organization) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Account_popup_txt_organization_cannot_be_empty,
            icon: 'error'
        }).then((result) => {
            document.getElementById("save").disabled = false;
        });
        return;
    }

    var data = {
        "username": username,
        "email": email,
        "projectId": projectId,
        "contractId": contractId,
        "districtId": districtId,
        "admUnitId": admUnitId,
        "schoolId": schoolId,
        "roleId": roleId,
        "organization": organization,
        "id": id,
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: '/account/register',
        type: "POST",
        data: data,
        success: function (result) {
            if (result == 'ok') {
                var messageSuccess = "";
                if ($("#id").val() > 0) {
                    messageSuccess = layoutText.Account_popup_txt_edit_successful;
                } else {
                    messageSuccess = layoutText.Account_popup_txt_successfully_created;
                }
                Swal.fire({
                    title: 'Success',
                    text: messageSuccess,
                    icon: 'success'
                }).then((result) => {
                    reloadList();
                    hideEditor();
                });
            } else {
                console.log(result)
                Swal.fire({
                    title: 'Error',
                    text: result,
                    icon: 'error'
                });
            }
            document.getElementById("save").disabled = false;
        },
        error: function (xhr, resp, text) {
            document.getElementById("save").disabled = false;
        }
    })
});

function getData() {
    $.getJSON('/account/GetMenuConfig?menu=Account', function (values) {
        var mapping = dynamicColumn(values)
        $("#thead").append(`<th>${layoutText.accountTblTxtActions}</th>`)
        $("#tfoot").append(`<th>${layoutText.accountTblTxtActions}</th>`)
        $('#account-list').DataTable({
            initComplete: onInitTableComplete,
            dom: 'lrtp',
            ordering: true,
            processing: true,
            serverSide: true,
            filter: true,
            ajax: {
                "url": "/account/GetListAccount",
                "type": "POST",
                "datatype": "json"
            },
            "columns": [
                ...mapping,
                {
                    "data": 'Id',
                    "render": getActionButton,
                },
            ],
        });
    })
}

function dynamicColumn(values) {
    return values.map(val => {
        $("#thead").append(`<th>${val.Name}</th>`)
        $("#tfoot").append(`<th>${val.Name}</th>`);
        switch (val.Key) {
            case "Id":
                return {
                    "data": val.Key, "name": val.Name, "autoWidth": true, visible: false
                };
            case "No":
                return {
                    "data": null, "name": "No", "autoWidth": true, orderable: false,
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                };
            default:
                return {
                    "data": val.Key, "name": val.Name, "autoWidth": true
                }
        }
    })
}

function getActionButton(data, type) {
    return `<button class="btn-aw edit"><a class="c-white" href="javascript:void(0)" onclick="editAccount(${data})"><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Account_btn_tooltip_edit}</span></a></button><button class="btn-aw delete"><a class="c-white" href="javascript:void(0)" onclick="deleteAccount(${data})"> <img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Account_btn_tooltip_delete}</span></a></button>`;
}

function onInitTableComplete() {
    $('#account-list tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" class="Search' + title + '" placeholder="' + layoutText.Account_tbl_txt_search + '" />');
        if (title == "Actions" || title == "No") {
            $(this).html("");
        }
    });
    $('#account-list tbody tr').each(function () {
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

    var r = $('#account-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#account-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function deleteAccount(id) {
    Swal.fire({
        title: 'Confirmation',
        text: layoutText.Account_popup_txt_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/account/DeleteAccount/' + id,
                type: "POST",
                data: {},
                success: function (result) {
                    if (result == 'ok') {
                        Swal.fire({
                            title: 'Success',
                            text: layoutText.Account_popup_txt_account_has_been_deleted,
                            icon: 'success'
                        }).then((result) => {
                            reloadList();
                            hideEditor();
                        });
                    }
                },
                error: function (xhr, resp, text) {
                    // console.log(xhr, resp, text);
                }
            });
        }
    });


}

$("#project-id").change((e) => {
    getAdmUnit($("#project-id").val());
    getContract($("#project-id").val());
    getSchool($("#project-id").val(), $("#admUnit-id").val());
});

$("#admUnit-id").change((e) => {
    getSchool($("#project-id").val(), $("#admUnit-id").val());
});

function getDistrict(projectId) {
    $.getJSON(`/account/GetDistrict?projectId=${projectId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#district-id").html(opt);
    });
}

function getAdmUnit(projectId) {
    $.getJSON(`/account/GetAdmUnit?projectId=${projectId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#admUnit-id").html(opt);
    });
}

function getContract(projectId) {
    $.getJSON(`/account/GetContract?projectId=${projectId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#contract-id").html(opt);
    });
}

function getSchool(projectId, districtId) {
    $.getJSON(`/account/GetSchool?projectId=${projectId}&districtId=${districtId}`, (res) => {
        var opt = '';
        for (let i = 0; i < res.length; i++) {
            opt += `<option value='${res[i].Id}'>${res[i].Name}</option>`;
        }
        $("#school-id").html(opt);
    });
}

function openModalAccount(reset) {
    var teacher = window.location.href.includes('teacher');
    initEditor(reset);
    setEditorTitle(!teacher ? layoutText.Account_form_title_txt_new_account : layoutText.Account_form_title_txt_new_teacher_account);
    $("#modal-account").modal("show");
}

function closeModalAccount() {
    $("#modal-account").modal("hide");
}

// $('#modal-account').on('hidden.bs.modal', function () {
//     const params = new Proxy(new URLSearchParams(window.location.search), {
//         get: (searchParams, prop) => searchParams.get(prop),
//     });
//     let id = params.id;
//     if (id) location.href = '/account'
// });

function editAccount(id) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `/get-account/${id}`,
        dataType: "json",
        success: function (response) {
            if (response.status != 'OK') {
                Swal.fire({
                    title: 'Success',
                    text: response.message,
                    icon: 'Success'
                });
                return;
            }

            let account = response.account;
            var teacher = window.location.href.includes('teacher');
            setEditorTitle(!teacher ? layoutText.Account_form_title_txt_edit_account : layoutText.Account_form_title_txt_edit_teacher_account);
            $("#save").html(layoutText.Account_form_btn_save);
            $("#id").val(account.id);
            $("#username").val(account.username);
            $("#email").val(account.email);
            $("#project-id").val(account.project);
            $("#contract-id").val(account.contract);
            $("#district-id").val(account.district);
            $("#admUnit-id").val(account.admUnit);
            $("#school-id").val(account.school);
            $("#role-id").val(account.role);
            $("#organization").val(account.organization);

            showEditor();
        },
        error: function (response) {
            console.log(response)
            Swal.fire({
                title: 'Error',
                text: response.responseJSON.message,
                icon: 'error'
            });
        }
    });
}

function initEditor(reset) {
    $("#id").val(0);
    $("#username").val('');
    $("#email").val('');
    if (reset) {
        $("#project-id").val('');
        $("#contract-id").val('');
    }
    $("#district-id").val('');
    $("#school-id").val('');
    $("#role-id").val('');
    $("#organization").val('');
}

function showEditor() {
    $("#modal-account").modal("show");
}

function setEditorTitle(title) {
    $('#modal-account .modal-title').text(title);
}

function reloadList() {
    $('#account-list').DataTable().ajax.reload();
}

function hideEditor() {
    $("#modal-account").modal("hide");
    initEditor();
}

function newAccount(reset) {
    var teacher = window.location.href.includes('teacher');
    initEditor(reset);
    setEditorTitle(!teacher ? layoutText.Account_form_title_txt_new_account : layoutText.Account_form_title_txt_new_teacher_account);
    $("#save").html(layoutText.Account_form_btn_create);
    $('#id').val(0);
    showEditor();
}


var inputUsername = jQuery("#username");
inputUsername.on('keyup keydown copy', function () {
    $("#username").keypress("input", function (e) {
        var k = e.keyCode,
            $notAllowSymbol = ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        if (!$notAllowSymbol) {
            return false;
        }
    })
    cekUsername();
})

function cekUsername() {
    var errUsername = "";
    const errUser1 = layoutText.Account_popup_txt_username_validation_error1;
    const errUser2 = layoutText.Account_popup_txt_username_validation_error2;
    var messageHtml = `<ul> <li> ${errUser1} </li> <li> ${errUser2} </li></ul>`;
    var user = document.getElementById('username');
    if (user.value.length < 6 || user.value.length > 25) {
        errUsername = `${errUser1} \n ${errUser2}`;
        $("#usernameNotif").html(messageHtml);
    } else {
        $("#usernameNotif").html("");
    }
    return errUsername;
}

var inputPassword = jQuery("#password");
inputPassword.on('keyup keydown copy', function () {
    $("#password").keypress("input", function (e) {
        var k = e.keyCode,
            $notAllowSymbol = ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        if (!$notAllowSymbol) {
            return false;
        }
    })
    cekPassword();
})

function cekPassword() {
    var errPassword = "";
    const err = layoutText.Login_popup_txt_password_must_be_at_least;
    var messageHtml = `<ul><li> ${err} </li></ul>`;
    var pass = document.getElementById('password');
    var textPass = $("#password").val();
    var cekPass = allowNumber(textPass);

    if (pass.value.length < 6 || pass.value.length > 25 || cekPass == null) {
        errPassword = err;
        $("#passwordNotif").html(messageHtml);
    } else {
        $("#passwordNotif").html("");
    }
    //return errPassword; this line is still not used
}

function allowNumber(s) {
    var rgx = /[0-9]/;
    return s.match(rgx);
}

var enterUserToPass = document.getElementById("username");
if (enterUserToPass != null) {
    enterUserToPass.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            $('#password').focus();
        }
    });
}

var enterPassToLogin = document.getElementById("password");
if (enterPassToLogin != null) {
    enterPassToLogin.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            document.getElementById("login").click();
        }
    });
}
