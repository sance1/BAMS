var arrAccess = {};
$("#create-form").submit(function (e) {
    e.preventDefault();
    var name = $("#create-name").val();
    var access = $("#create-access").val();
    var id = $("#id").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var rolePermission = [];
    var key = Object.keys(arrAccess);
    $.each(key, function (idx, item) {
        rolePermission.push({
            key: item,
            value: arrAccess[item]
        })
    })

    if (!name) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Role_popup_txt_name_cannot_be_empty,
            icon: 'error'
        });
        return;
    }

    if (!access) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Role_popup_txt_access_level_cannot_be_empty,
            icon: 'error'
        });
        return;
    }

    var data = {
        name: name,
        "id": id,
        "accessLevel": access,
        "rolePermission": JSON.stringify(rolePermission),
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: '/role/CreateOrUpdateRole',
        type: "POST",
        data: data,
        success: function (result) {
            if (result == 'ok') {
                Swal.fire({
                    title: 'Success',
                    text: "Success",
                    icon: 'success'
                }).then((result) => {
                    location.href = '/role';
                });
            }
        },
        error: function (xhr, resp, text) {
            // console.log(xhr, resp, text);
        }
    })
});

function getData() {
    var columnCount = 0;
    var tbl = $('#role-list').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: getDataSourceRole(),
        columns: [
            {data: "Id", name: "Id", visible: false},
            {data: "Name", name: "Name"},
            {data: "AccessLevel", name: "AccessLevel"},
            {
                data: 'Id',
                "render": getActionButton,
            },
        ]
    });
}

function getDataSourceRole() {
    return {
        "url": "/role/GetListRole",
        "type": "POST",
        "datatype": "json"
    }
}

function getActionButton(data, type) {
    return `<button class="btn-aw edit"><a class="c-white" href="/role/update/${data}"><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Role_btn_tooltip_edit}</span> </a></button><button class="btn-aw delete"><a class="c-white" href="Javascript:void(0)" onclick="deleteRole(${data})"> <img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Role_btn_tooltip_delete}</span></a></button>`;
}

function onInitTableComplete() {
    $('#role-list tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder=" ' + layoutText.Role_tbl_txt_search + '" />');
        if (title == "Actions") {
            $(this).html("");
        }
    });
    $('#role-list tbody tr').each(function () {
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

    var r = $('#role-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#role-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function getRolePermission(id) {
    return $.getJSON('/role/GetListRolePermission/' + id, function (data) {
        return data
    })
}

function getMenu() {
    var id = $("#id").val();
    getRolePermission(id)
        .then(val => {
            $.each(val, function (idx, item) {
                arrAccess[item.Group] = item.Access;
            })

            $('#tbl-create-role').DataTable({
                initComplete: function () {

                },
                dom: 'lrtp',
                ordering: true,
                processing: true,
                serverSide: true,
                filter: false,
                ajax: getDataSourceMenu(),
                "columns": [
                    {data: "Name", name: "Name"},
                    {data: "Group", name: "Group"},
                    {
                        "render": getCheckList,
                    },
                ]
            });
        })
}

function getDataSourceMenu() {
    return {
        "url": "/role/GetListMenu",
        "type": "POST",
        "datatype": "json"
    }
}

function getCheckList(data, type, row) {
    var permission = arrAccess[row.Group];
    if (!permission)
        return `<input type="checkbox" value="${row.Id}" onchange="onChangeAccess(this,'${row.Group}',${row.Permission})" />`;
    var checked = (permission & row.Permission) === row.Permission;
    if (checked)
        return `<input type="checkbox" value="${row.Id}" checked onchange="onChangeAccess(this,'${row.Group}',${row.Permission})" />`;
    else
        return `<input type="checkbox" value="${row.Id}" onchange="onChangeAccess(this,'${row.Group}',${row.Permission})" />`;
}

function onChangeAccess(e, group, access) {
    var data = arrAccess[group]
    if (!data) {
        arrAccess[group] = 0
    }
    if (e.checked) {
        arrAccess[group] += access
    } else {
        arrAccess[group] -= access
    }
}

function deleteRole(id) {

    Swal.fire({
        title: layoutText.Role_popup_txt_confirmation,
        text: layoutText.Role_popup_txt_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/role/DeleteRole/' + id,
                type: "POST",
                data: {},
                success: function (result) {
                    if (result == 'ok') {
                        Swal.fire({
                            title: 'Success',
                            text: layoutText.Role_popup_txt_success,
                            icon: 'success'
                        }).then((result) => {
                            location.href = '/role';
                        });
                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: result,
                            icon: 'error'
                        });
                    }
                },
                error: function (xhr, resp, text) {
                    // console.log(xhr, resp, text);
                }
            })

        }
    });

}

function create() {
    location.href = `role/create`;
}