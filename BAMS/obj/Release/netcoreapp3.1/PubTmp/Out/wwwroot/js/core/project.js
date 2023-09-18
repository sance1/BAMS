const Toast = Swal.mixin({
    toast: true,
    position: 'top',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});


$(document).ready(function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let act = params.act;
    if (act === "new") newProject()
    let id = params.uid;
    if(id){
        openModalProject();
    }
    var tbl = initDataTable();
    
    $("#countryId").change(getListProvince);
    
});

function initDataTable() {
    $('#tbl').DataTable({
        autoWidth: true,
        processing: true,
        dom: 'lrtp',
        filter: true,
        ordering: true,
        serverSide: true,
        ajax: getDataSource(),
        columns: [
            { data: null, name: "No", autoWidth: true, orderable: false},
            { data: "Name", name: "Name", autoWidth: true },
            { data: "PartnerName", name: "PartnerName", autoWidth: true },
            { data: "ContactPerson", name: "ContactPerson", autoWidth: true },
            { data: "PartnerPIC", name: "PartnerPIC", autoWidth: true },
            { data: "Contracts", name: "Contracts", autoWidth: true },
            { data: "Districts", name: "Districts", autoWidth: true },
            { data: "Schools", name: "Schools", autoWidth: true },
            { data: "Remarks", name: "Remarks", autoWidth: true },
            { data: null, name: "Remarks", autoWidth: true, orderable: false },
            { data: "Id", name: "Id", visible: false },
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
            {
                targets: -2,
                render: renderButton
            }
        ],
        initComplete: onTableInitComplete
    });
}

function onTableInitComplete() {
    $('#tbl tr.search th').each(function (i) {
        if ($(this).is('[no-search]')) {
            $(this).html('');
            return;
        }
        var title = $(this).text();
        $(this).html('<input type="text" class="Search' + title + '" placeholder="' + layoutText.Project_popup_tbl_txt_search +'" />');
    });
    this.api().columns().every(function () {
        var that = this;
        if(this.footer() != null){
            $('input', this.footer()).on('keyup change clear', delay(function () {
                if (that.search() !== this.value) {
                    that.search(this.value).draw();
                }
            }));
        }
    });

    //move from tfoot to thead
    $('#tbl tr.search').appendTo('#tbl thead');
}

function getDataSource() {
    return {
        url: "/project/GetListProject",
        type: "POST",
        datatype: "json"
    }
}

function renderButton(data, type, row, meta) {
    let str = `<button class="btn-aw pink" onclick="go_contract(${data.Id})" ><img src="/img/icon/create-contract.png" /> <span class="tooltipText">${layoutText.Project_btn_tooltip_contract}</span></button><button class="btn-aw edit" onclick="editProject(${data.Uid})" ><img src="/img/icon/edit.png" /> <span class="tooltipText">${layoutText.Project_btn_tooltip_edit}</span></button>`;
    if (data.Contracts == 0 && data.Districts == 0 && data.Schools == 0) {
        str += `<button class="btn-aw delete" onclick="deleteProject(${data.Uid})" ><img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Project_btn_tooltip_delete}</span></button>`;
    } else {
        str += `<button class="btn-aw disable" onclick="errorDelete()"><img src="/img/icon/delete.png" /> <span class="tooltipText">${layoutText.Project_btn_tooltip_delete}</span></button>`;
    }
    return str;
}

$('#modal-project').on('hidden.bs.modal', function () {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    let id = params.uid;
    if (id) location.href = '/projects'
});

function errorDelete() {
    Swal.fire({
        title: 'Error',
        html: layoutText.Project_popup_delete_that_data_first,
        icon: 'error'
    });
}

function reloadList() {
    $('#tbl').DataTable().ajax.reload(null, false);
}

function initEditor() {
    $("#uid").val(0);
    $("#countryId").val("");
    $("#provinceId").val("");
    $("#partnerName").val('');
    $("#contactPerson").val('');
    $("#partnerPIC").val('');
    $("#remarks").val('');
}

function hideEditor(){
    $("#modal-project").modal("hide");
    initEditor();
}


function newProject(){
    initEditor();
    $('#editor-title').text(layoutText.Project_form_new_project);
    $("#modal-project").modal("show");
}

function editProject(uid) {
    initEditor();

    $.ajax({
        type: "GET",
        url: `get-project/${uid}`,
        dataType: "json",
        success: function (response) {
            if (response.status != 'OK') {
                Swal.fire({
                    title: 'Error',
                    html: response.message,
                    icon: 'error'
                });
                return;
            }
            
            let project = response.project;
            $('#editor-title').text(layoutText.Project_form_edit_project)
            $('#uid').val(project.Uid);

            $('#countryId').val(project.CountryId).change();
            setTimeout(() => $('#provinceId').val(project.ProvinceId), 1000)
            $('#partnerName').val(project.PartnerName);
            $('#contactPerson').val(project.ContactPerson);
            $('#partnerPIC').val(project.PartnerPIC);
            $('#remarks').val(project.Remarks);        
            $('#modal-project').modal('show');

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

function deleteProject(uid) {
    Swal.fire({
        title: layoutText.Project_popup_delete_confirmation,
        text: layoutText.Project_popup_are_you_sure,
        icon: 'warning',
        showDenyButton: true,
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: `/project/delete/${uid}`,
                dataType: "json",
                success: function (response) {
                    if (response.status == 'OK') {
                        reloadList();
                        Toast.fire({
                            icon: 'success',
                            title: layoutText.Project_toast_success_deleted
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


function saveProject() {
    let uid = parseInt($("#uid").val());

    let project = {
        uid: uid,
        appUid: parseInt($("#appId").val()),
        name: $("#countryId").val() + "-" + $("#provinceId").val(),
        partnerName: $("#partnerName").val(),
        contactPerson: $("#contactPerson").val(),
        PartnerPIC: $("#partnerPIC").val(),
        remarks: $("#remarks").val(),
        countryId: parseInt($("#countryId").val()),
        provinceId: parseInt($("#provinceId").val())
    };

    let url = uid == 0
        ? '/create-project'
        : '/update-project';

    let validationResults = validateInput(project);
    if (validationResults.length > 0) {
        Swal.fire({
            title: 'Data Validation',
            html: validationResults.join('<br>'),
            icon: 'error'
        });
        return;
    }

    sendRequest(url, JSON.stringify(project))
}

function validateInput(input) {
    let results = [];
    if (input.name == "") {
        results.push(layoutText.Project_popup_name_cannot_empty);
    }
    if (input.partnerName == "") {
        results.push(layoutText.Project_popup_partner_name_cannot_empty);
    }
    if (input.contactPerson == "") {
        results.push(layoutText.Project_popup_contact_person_cannot_empty);
    }
    return results;
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
                    title: layoutText.Project_toast_has_been_saved_successfully
                });
                return;
            }
            Swal.fire({
                title: 'Error',
                text: response.message,
                icon: 'error'
            });
        },
        error: function(response){
            Swal.close();
            Swal.fire({
                title: 'Error',
                text: response.responseJSON.message,
                icon: 'error'
            });
        }
    });
}


function go_contract(id) {
    location.href = `/contract/${id}`;
}

function getListProvince() {   
    var countryId = parseInt($("#countryId").val());
    var url = `/Province/GetByCountryId/${countryId}`;
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
           
            if (response.province.length > 0) {
                for (var i = 0; i < response.province.length; i++) {
                    var data = response.province[i];                    
                    $("#provinceId").append($("<option />").val(data.Id).text(data.Name));
                }               
            } else {
                $("#provinceId").html("");
            }
            

                        
        },
        error: function () {
            alert("Get List Province function error")
        }
    });
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});