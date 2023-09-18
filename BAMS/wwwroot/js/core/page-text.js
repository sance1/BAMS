$(document).ready(function () {

    initDataTable();

});

function initDataTable() {
    $('#page-text-list').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            "url": "/PageText/GetListPageText",
            "type": "POST",
            "datatype": "json"
        },
        columns: [
            {data: "Key", name: "Key", autoWidth: true},
            {data: "Text", name: "Text", autoWidth: true},
            {data: "LanguageCode", name: "LanguageCode", autoWidth: true},

            {data: null, name: "Remarks", autoWidth: true, orderable: false},
            {data: "Id", name: "Id", visible: false},
        ],
        columnDefs: [
            {
                targets: -2,
                render: function (data, type, row, meta) {
                    return `<button class="btn-aw edit" onclick="edit(${data.Uid})" ><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Pagetexts_btn_tooltip_edit}</span> </button><button class="btn-aw delete" onclick="remove(${data.Uid})" ><img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Pagetexts_btn_tooltip_delete}</span></button>`;
                }
            }
        ]
    });
}

function onInitTableComplete() {
    $('#page-text-list tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + layoutText.Pagetexts_tbl_txt_search + '"/>');
        if (title == "Actions") {
            $(this).html("");
        }
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

    var r = $('#page-text-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#page-text-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function create() {
    location.href = `/new-PageText`;
}

function go_contract(id) {
    location.href = `/contract/${id}`;
}

function edit(id) {
    location.href = `/PageText/${id}`;
}

function remove(id) {
    Swal.fire({
        title: layoutText.Pagetexts_popup_txt_confirmation,
        text: layoutText.Pagetexts_popup_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/PageText/delete/${id}`,
                dataType: "json",
                success: function (response) {
                    if (response.message == "success") {
                        Swal.fire({
                            title: 'Success',
                            text: response.message,
                            icon: 'success'
                        }).then((result) => {
                            location.reload();
                        });

                        return;
                    }

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