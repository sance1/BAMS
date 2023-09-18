$(document).ready(function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.contractUid;
    let page = params.page;
    if (id && page === 'uploadactivationcode') openModalUpload();
    initDataTable();
});

function getDataSource() {
    return {
        "url": "/contract/GetListContract?projectId=0",
        "type": "POST",
        "datatype": "json"
    }
}

function initDataTable() {
    $('#tbl').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: getDataSource(),
        columns: [
            {data: null, name: "No", autoWidth: true, orderable: false},
            {data: "Name", name: "Name", autoWidth: false},
            {data: "RemarksRequest", name: "RemarksRequest", autoWidth: false},
            {data: "CodeRequestDate", name: "CodeRequestDate", autoWidth: false},
            {data: "CodeUploadDate", name: "CodeUploadDate", autoWidth: false},
            {data: "ActivationCodes", name: "ActivationCodes", autoWidth: false},
            {data: "UploadedCode", name: "UploadedCode", autoWidth: false},
            {data: "StatusUploadText", name: "StatusUploadText", autoWidth: false},
            {data: "Uid", name: "Uid", visible: false},
            {data: null, name: "Remarks", autoWidth: true, orderable: false}
        ],
        fixedColumns: {
            right: 1
        },
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            renderUploadCode(),
            renderDate()
        ]
    });
}

function renderDate() {
    return {
        targets: [3, 4],
        className: 'dt-body-center',
        render: function (data, type, row, meta) {
            if (data == null) return '';
            var dt = new Date(data);
            var timestamp = dt.getTime() - dt.getTimezoneOffset() * 60000;
            dt = new Date(timestamp);
            return dateFormat(dt, "dd-mm-yyyy");
        }
    }

}

function renderUploadCode() {
    return {
        targets: -1,
        render: function (data, type, row, meta) {
            let str = '';
            if (data.UploadedCode > 0 && data.ActivationCodes == data.UploadedCode) {
                str += `<button class="btn-aw done"><i class="fa fa-check" aria-hidden="true"></i> <span class="tooltipText">Done</span></button>`;
            } else {
                str += `<button class="btn-aw light_blue" onclick="openModalUpload(${data.ActivationCodeRequestId})" ><img src="/img/icon/distribute-activation-code.png" /> <span class="tooltipText">${layoutText.Contract_btn_tooltip_upload_codes}</span></button>`;
            }
            return str;
        }
    }
}

function uploadCodes(id) {
    location.href = `/contract/all?page=uploadactivationcode&contractUid=${id}`;
}

function onInitTableComplete() {
    $('#tbl tfoot th').each(function () {
        var title = $(this).text();
        $(this).html(`<input type="text" class="Search ${title}" placeholder="${layoutText.Contract_tbl_txt_search}" />`);
        if (title == "Actions" || title == "No") {
            $(this).html("");
        }
    });
    $('#tbl tbody tr').each(function () {
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
    var r = $('#tbl tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#tbl thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function initEditor() {
    $("#acReqUid").val(0);
    $("#contractName").val('');
    $("#requestDate").val('');
    $("#requestedCodes").val('');
    $("#remarks").val('');
    $("#file_upload").val('');
    $("#file-name").html(layoutText.Contract_form_txt_no_file_chosen)
}

function openModalUpload(id) {
    initEditor();

    var row = $(tbl).DataTable().rows().data().toArray().find(a => a.ActivationCodeRequestId == id);
    $("#acReqUid").val(id);
    $("#contractName").val(row.Name);
    $("#requestDate").val(dateFormat(row.CodeRequestDate.substring(0, 10), "dd-mm-yyyy"));
    $("#requestedCodes").val(row.ActivationCodes);
    $("#remarks").val(row.RemarksRequest);

    $("#modal-upload-ac").modal("show");
}

function closeModalUpload() {
    initEditor();
    $("#modal-upload-ac").modal("hide");
}

$('#modal-upload-ac').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.contractUid;
    let page = params.page;
    if (id && page == 'uploadactivationcode') location.href = '/contract/all'
});

function saveUpload() {
    if ($("#file_upload")[0].files.length == 0) {
        Swal.fire({
            title: 'Error',
            text: layoutText.Contract_popup_txt_select_file,
            icon: 'error'
        });
        return;
    }

    var formdata = new FormData();
    formdata.append('file', $("#file_upload")[0].files[0]);

    let uid = $("#acReqUid").val();

    var url = `/contract/uploadacrequest/${uid}`;

    $.ajax({
        type: "POST",
        url: url,
        data: formdata,
        cache: false,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.status == 0) {
                Swal.fire({
                    title: 'Processing',
                    text: response.message,
                    icon: 'success'
                }).then((result) => {
                    initEditor();
                    closeModalUpload();
                    $('#tbl').DataTable().ajax.reload();
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

inputElement = document.getElementById('file_upload')
labelElement = document.getElementById('file-name')
inputElement.onchange = function (event) {
    var path = inputElement.value;
    if (path) {
        labelElement.innerHTML = path.split(/(\\|\/)/g).pop()
    }
}