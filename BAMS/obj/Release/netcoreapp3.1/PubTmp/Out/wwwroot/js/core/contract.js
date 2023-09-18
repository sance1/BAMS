$(document).ready(function () {

    $("#project-sel").change(function (e) {
        let projectId = $(this).val();
        location.href = `/contract/${projectId}`;
    });

    $('#contract-editor .save-button').click(saveContract);

    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.projectUid;
    let contractUid = params.contractUid;
    let page = params.page;
    let act = params.act;
    const projectUid = $("#project-uid").val()
    if (act === "new") newContract(projectUid)
    if (id && contractUid && page == "activatiocoderequest") {
        openModalContractAc();
    } else if (id) {
        openModalContract();
    }
    initDataTable()
    $("#save").click(saveContract);

    getAutoContractName();
});

function getAutoContractName() {
    var dt = new Date($("#startDate").val());
    var month = "";
    var year = "";
    var auto = "AUTO/";
    var projectName = $("#projectUid option:selected").text();
    if (dt == "Invalid Date") {
        dt = new Date();
    }
    month = (dt.getMonth() + 1).toString().padStart(2,"0") + "/";
    year = dt.getFullYear() + "/";


    var modalTItle = $('#contract-editor .modal-title').text();
    if (modalTItle == "Edit Contract") {
        var firstData = $("#dataAuto").html();
        var splitfirstData = firstData.split("/");
        auto = splitfirstData[0] + "/";
        projectName = $("#projectUid option:selected").text();
    }

    var dataAuto = auto + month + year + projectName;
    $("#dataAuto").html(dataAuto);
}


$('#contract-editor').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.projectUid;
    const projectId = $("#project-id").val()
    if (id) location.href = '/contract/' + projectId
});

function initDataTable() {
    $('#contract-list').DataTable({
        initComplete: onInitTableComplete,
        dom: 'lrtp',
        ordering: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: getDataSource(),
        columns: [
            {data: null, name: "No", autoWidth: true, orderable: false},
            {data: "Name", name: "Name"},
            {data: "StartDate", name: "StartDate"},
            {data: "EndDate", name: "EndDate"},
            {data: "CodeRequestDate", name: "CodeRequestDate"},
            {data: "CodeUploadDate", name: "CodeUploadDate"},
            {data: "ActivationCodes", name: "ActivationCodes"},
            {data: "UploadedCode", name: "UploadedCode"},
            {data: "StatusText", name: "StatusText"},
            {data: "Remarks", name: "Remarks"},
            {data: "Id", name: "Id", "visible": false},
            {data: "ProjectName", name: "ProjectName", "visible": false},
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
            renderButtonStatus(),
            renderDate()

        ],
        rowCallback: function (row, data, index) {
            if (data.DeletedDate) {
                $(row).addClass('row-deleted');
            }
        },
    });
}

function renderDate() {
    return {
        targets: [2, 3, 4, 5],
        className: 'dt-body-center',
        render: function (data, type, row, meta) {
            if (data == null) return "";
            var dt = new Date(data);
            var timestamp = dt.getTime() - dt.getTimezoneOffset() * 60000;
            dt = new Date(timestamp);
            return dateFormat(dt, "dd-mm-yyyy");
        }
    }
}

function renderButtonStatus() {
    return {
        targets: -1,
        render: function (data, type, row, meta) {
            if (data.DeletedDate) {
                return 'Has Been Deleted';
            }
            let str = '';
            if (data.CodeRequestDate == null) {
                str += `<button class="btn-aw light_blue" onclick="reqAC('${data.Uid}')"><img src="/img/icon/distribute-activation-code.png"> <span class="tooltipText">${layoutText.Contract2_btn_tooltip_request_activation_code}</span></button>`;
            } else {
                str += `<button class="btn-aw disable"><img src="/img/icon/distribute-activation-code.png"> <span class="tooltipText">${layoutText.Contract2_btn_tooltip_request_activation_code}</span></button>`;
            }
            if (data.CodeUploadDate == null && data.CodeRequestDate == null) {
                str += `<button class="btn-aw edit" onclick="editContract('${data.Uid}')"><img src="/img/icon/edit.png"> <span class="tooltipText">${layoutText.Contract2_btn_tooltip_edit}</span></button>`;
            } else {
                str += `<button class="btn-aw disable"><img src="/img/icon/edit.png"> <span class="tooltipText">${layoutText.Contract2_btn_tooltip_edit}</span></button>`;
            }
            if (data.UploadedCode == 0) {
                str += `<button class="btn-aw delete" onclick="deleteContract('${data.Uid}')"><img src="/img/icon/delete.png"> <span class="tooltipText">${layoutText.Contract2_btn_tooltip_delete}</span></button>`;
            }
            return str;
        }
    }
}

function getDataSource() {
    const projectId = $("#project-id").val()
    return {
        "url": "/contract/GetListContract?projectId=" + projectId + "&ignoreFilter=false",
        "type": "POST",
        "datatype": "json"
    }
}

function onInitTableComplete() {
    let remarksIndex = -1;
    $('#contract-list tfoot th').each(function (i) {
        var title = $(this).text();
        if (title == "Remarks") remarksIndex = i;
        $(this).html('<input type="text" class="contract' + title + '" placeholder="' + layoutText.Contract2_tbl_txt_search + ' " />');
        var excludes = ["Actions", "No"];
        if (excludes.includes(title)) {
            $(this).html("");
        }
    });
    $('#contract-list tbody tr').each(function () {
        $('td', this).each(function (i) {
            var value = $(this).text();
            if (!isNaN(+value) && i > 0 && i != remarksIndex) {
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

    var r = $('#contract-list tfoot tr');
    r.find('th').each(function () {
        $(this).css('padding', 8);
    });
    $('#contract-list thead').append(r);
    $('#search_0').css('text-align', 'center');
}

function openModalContractAc() {
    $("#modal-contract-ac").modal("show");
}

function closeModalContractAc() {
    $("#modal-contract-ac").modal("hide");
}

$('#modal-contract-ac').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.projectUid;
    const projectId = $("#project-id").val()
    if (id) location.href = '/contract/' + projectId
});


/********************/

const Toast = Swal.mixin({
    toast: true,
    position: 'top',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});


function reloadList() {
    $('#contract-list').DataTable().ajax.reload(null, false);
}

function initEditor() {
    $("#uid").val(0);
    $("#projectUid").val(0);
    $("#name").val('');
    $("#startDate").val(new Date().toISOString().substring(0, 10)).change();
    $("#endDate").val(new Date().toISOString().substring(0, 10)).change();
    $("#activationCodes").val('');
    $("#remarks").val('');
}

function showEditor() {
    $('#contract-editor').modal('show');
}

function hideEditor() {
    $("#contract-editor").modal("hide");
    initEditor();
}

function setEditorTitle(title) {
    $('#contract-editor .modal-title').text(title);
}

function newContract(projectUid) {
    $("#projectUid")[0].disabled = false;
    initEditor();
    setEditorTitle(layoutText.Contract2_form_title_new_contract);
    $('#projectUid').val(projectUid);
    getAutoContractName();
    showEditor();
}

function editContract(uid) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `/get-contract/${uid}`,
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
            $("#projectUid")[0].disabled = true;
            let contract = response.contract;
            setEditorTitle(layoutText.Contract2_form_title_edit_contract);
            $('#uid').val(contract.uid);
            $('#projectUid').val(contract.projectUid);
            $('#name').val(contract.name);
            $('#startDate').val(contract.startDate).change()
            $('#endDate').val(contract.endDate).change();
            $('#activationCodes').val(contract.activationCodes);
            $('#remarks').val(contract.remarks);

            var name = $("#name").val();
            var cekCharMinus = charMinus(name);
            if (cekCharMinus != null) {
                var splitName = name.split("-");
                var lastName = splitName[splitName.length - 1];
                $("#name").val(lastName);
                splitName.pop();
                var firstname = splitName.join("-");
                $("#dataAuto").html(firstname);
            } else {
                $("#dataAuto").html(name);
                $("#name").val("");
            }
            
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

function charMinus(s) {
    var rgx = /[-]/;
    return s.match(rgx);
}

function deleteContract(uid) {
    Swal.fire({
        title: layoutText.Contract2_popup_txt_delete_confirmation,
        text: layoutText.Contract2_popup_txt_want_to_delete,
        icon: 'warning',
        showDenyButton: true,
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/delete-contract/${uid}`,
                dataType: "json",
                success: function (response) {
                    if (response.status == 'OK') {
                        reloadList();
                        Toast.fire({
                            icon: 'success',
                            title: layoutText.Contract2_popup_deleted_successfully
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

function saveContract() {

    let uid = parseInt($("#uid").val());
    var name = $("#name").val();

    var modalTItle = $('#contract-editor .modal-title').text();
    if (modalTItle == "Edit Contract") {
        var firstNameContract = $("#dataAuto").html();
        var LastNameContract = name;
        name = firstNameContract + "-" + LastNameContract;
    }
    
    
    let contract = {
        uid: uid,
        projectUid: parseInt($("#projectUid").val()),
        name: name,
        startDate: new Date($("#startDate").val()),
        endDate: new Date($("#endDate").val()),
        activationCodes: parseInt($("#activationCodes").val()) || 0,
        remarks: $("#remarks").val(),
    };

    let validationResult = validateInput(contract)
    if (validationResult.length > 0) {
        Swal.fire({
            title: layoutText.Contract2_popup_data_validation,
            html: validationResult.join('<br>'),
            icon: 'error'
        });
        return;
    }

    let url = uid == 0
        ? '/create-contract'
        : '/update-contract';

    sendRequest(url, JSON.stringify(contract));
}

function validateInput(input) {
    let err = [];
    if (input.projectUid == "") {
        err.push(layoutText.Contract2_popup_txt_project_cannot_empty);
    }
    if (input.name == "") {
        err.push(layoutText.Contract2_popup_txt_contract_name_cannot_empty);
    }
    if (input.startDate == "Invalid Date") {
        err.push(layoutText.Contract2_popup_txt_start_date_invalid);
    }
    if (input.endDate == "Invalid Date") {
        err.push(layoutText.Contract2_popup_txt_end_date_invalid);
    }
    if (input.startDate > input.EndDate) {
        err.push(layoutText.Contract2_popup_txt_must_be_less_than_end_date);
    }
    if (input.activationCodes <= 0) {
        err.push(layoutText.Contract2_popup_txt_quantity_cannot_empty);
    }

    return err;
}

function sendRequest(url, payload) {
    Swal.showLoading();

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: payload,
        success: function (response) {
            Swal.close();
            if (response.status == "OK") {
                reloadList();
                hideEditor();
                Toast.fire({
                    icon: 'success',
                    title: layoutText.Contract2_popup_txt_saved_successfully
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

function to_project(id) {
    location.href = `/projects`;
}

function reqAC(id) {
    //location.href = `/contract/@projectId?projectUid=@projectId&contractUid=${id}&page=activatiocoderequest`;
    openModalContractAc();
    var row = $("#contract-list").DataTable().rows().data().toArray().find(a => a.Uid == id);
    $("#projectId").val(row.ProjectId);
    $("#contractId").val(row.Id);
    $("#contract-name").val(row.Name);
    $("#remarksAc").val(row.RemarksRequest);
}

function saveAc() {
    let data = {
        ProjectId: parseInt($("#projectId").val()) || 0,
        ContractId: parseInt($("#contractId").val()) || 0,
        Remarks: $("#remarksAc").val(),
    };

    let err = [];
    if (data.ProjectId == "" || data.ProjectId <= 0) {
        err.push(layoutText.Contract2_popup_txt_project_cannot_empty_when_save);
    }
    if (data.ContractId == "" || data.ContractId <= 0) {
        err.push(layoutText.Contract2_popup_contract_cannot_empty_when_save);
    }
    if (err.length > 0) {
        Swal.fire({
            title: 'Error',
            text: err.join("<br/>"),
            icon: 'error'
        });
        return;
    }

    let url = "/contract/createacrequest";
    let id = $("#acReqId").val();
    if (id && id != "") {
        data.Id = parseInt(id);
        url = "/contract/updateacrequest"
    }

    Swal.showLoading();

    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (response) {
            Swal.close();
            if (response.status == 0) {
                Swal.fire({
                    title: 'Success',
                    text: response.message,
                    icon: 'success'
                }).then((result) => {
                    closeModalContractAc();
                    reloadList();
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