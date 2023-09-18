$("#create-form").submit(function (e) {
    e.preventDefault();
    var username = $("#name").val();
    var password = $("#group").val();
    var projectId = $("#permission").val();
    var menuUrl = $("#menuUrl").val();
    var menuOrder = $("#menuOrder").val();
    var id = $("#id").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (!username) {
        alert(layoutText.Access_permissions_popup_txt_enter_name)
        return;
    }
    if (!password) {
        alert(layoutText.Access_permissions_popup_txt_enter_group)
        return;
    }

    if (!projectId) {
        alert(layoutText.Access_permissions_popup_txt_enter_permission)
        return;
    }

    var data = {
        "name": username,
        "group": password,
        "permission": projectId,
        "menuUrl": menuUrl,
        "menuOrder": menuOrder,
        "id": id,
        "__RequestVerificationToken": token
    }
    $.ajax({
        url: '/menu/CreateOrUpdateMenu',
        type: "POST",
        data: data,
        success: function (result) {
            if (result == 'ok') {
                Swal.fire({
                    title: 'Success',
                    text: "Success",
                    icon: 'success'
                }).then((result) => {
                    location.href = '/menu'
                });
            } else {
                Swal.fire({
                    title: 'Error',
                    text: result.message,
                    icon: 'error'
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
    var tbl = $('#menu-list').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            "url": "/menu/GetListMenu",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [

            {"data": "Id", "name": "Id", "autoWidth": true, "visible": false},
            {"data": "Name", "name": "Name", "autoWidth": true},
            {"data": "Group", "name": "Group", "autoWidth": true},
            {"data": "Permission", "name": "Permission", "autoWidth": true},
            {
                "data": 'Id',
                "render": getActionButton,
            },
        ]
    });
}

function getActionButton(data, type) {
    return `<button class="btn-aw edit"><a href="/menu?id=${data}" class="c-white"><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Access_permissions_btn_tooltip_edit}</span>  </a></button><button class="btn-aw delete"><a href="javascript:void(0)" class="c-white" onclick="deleteMenu(${data})"> <img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Access_permissions_btn_tooltip_delete}</span></a></button>`;
}

function onInitTableComplete() {
    $('#menu-list tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + layoutText.Access_permissions_tbl_txt_search + '" class="' + layoutText.Access_permissions_tbl_txt_search + ' " />');
        if (title == "Actions" || title == "Id") {
            $(this).html("");
        }
    });
    $('#menu-list tbody tr').each(function () {
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

    var r = $('#menu-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#menu-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function actionEdit(data) {
    location.href = "/menu/update/" + data;
}

function deleteMenu(id) {
    Swal.fire({
        title: 'Confirmation',
        text: layoutText.Access_permissions_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/menu/DeleteMenu/' + id,
                type: "POST",
                data: {},
                success: function (result) {
                    if (result == 'ok') {
                        Swal.fire({
                            title: 'Success',
                            text: "Success",
                            icon: 'success'
                        }).then((result) => {
                            location.href = '/menu'
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
            });
        }
    });


}

$("#permission").on("input", function () {
    if ($("#permission").val() == 8) {
        document.getElementById("menuUrl").disabled = false;
        document.getElementById("menuOrder").disabled = false;
    } else {
        document.getElementById("menuUrl").disabled = true;
        document.getElementById("menuOrder").disabled = true;
    }
});

function openModalMenu() {
    $("#modal-menu").modal("show");
    if ($("#permission").val() == 8) {
        document.getElementById("menuUrl").disabled = false;
        document.getElementById("menuOrder").disabled = false;
    } else {
        document.getElementById("menuUrl").disabled = true;
        document.getElementById("menuOrder").disabled = true;
    }
}

function closeModalMenu() {
    $("#modal-menu").modal("hide")
}

$('#modal-menu').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.id;
    if (id) location.href = '/menu'
});
