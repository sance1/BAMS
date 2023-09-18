const params = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => new URLSearchParams(window.location.search).get(prop),
});

$(document).ready(function () {

    getInbox(params.search ?? "", params.page ?? 0, params.pageSize ?? 5);
    $("#textMailSearch").keyup(function (e) {
        let code = e.key;
        if (code == "Enter") searchMail(params.page ?? 0, params.pageSize ?? 5);
    });
});

function searchMail(page = 0, pageSize = 5) {
    let src = $("#textMailSearch").val();
    getInbox(src, page, pageSize);
}

function getInbox(search = "", page = 0, pageSize = 5) {
    Swal.showLoading();
    $.ajax({
        type: "GET",
        url: `/message/getinbox?search=${search}&page=${page}&pageSize=${pageSize}`,
        dataType: "json",
        success: function (response) {
            Swal.close();
            dataMail = response.Data;
            let str = '';
            for (let i = 0; i < dataMail.length; i++) {
                let d = dataMail[i];
                var mydate = new Date(d.CreateDate);
                var dt = dateFormat(mydate, "dd/mm/yyyy HH:MM")
                let isRead = d.IsRead ? "read" : "unread";
                str += `<div class="list-message ${isRead}" onclick="readMail(this, ${d.Uid})">
                                <h3>${d.Sender.Name} ${d.Sender.Role === 'Admin' ? '(Admin)' : ''}</h3>
                                <h3>${d.Title}</h3>
                                <p>${d.Body}...</p>
                                <span>${dt}</span>
                            </div>`;
            }

            str += response.Prev >= 0 ? `<button class="btn-orange btn-aw" onclick="searchMail(${response.Prev})"> ${layoutText.Message_btn_prev}</button>` : `<button class="btn-orange btn-aw disable"> ${layoutText.Message_btn_prev}</button>`;
            str += response.Next > 0 ? `<button class="btn-orange btn-aw" onclick="searchMail(${response.Next})"> ${layoutText.Message_btn_next}</button>` : `<button class="btn-orange btn-aw disable"> ${layoutText.Message_btn_next}</button>`;
            $("#inbox").html(str);
            const url = new URL(window.location);
            url.searchParams.set('search', search);
            url.searchParams.set('page', page);
            url.searchParams.set('pageSize', pageSize);
            window.history.pushState({}, '', url);
        },
        error: function (response) {
            Swal.close();
            Swal.fire({
                title: 'Error',
                text: response.message,
                icon: 'error'
            });
        }
    });

}

function readMail(obj, uid) {
    Swal.showLoading();
    $.ajax({
        type: "GET",
        url: `/message/getmessage?uid=${uid}`,
        dataType: "json",
        success: function (response) {
            Swal.close();
            $(obj).removeClass("unread");
            $(obj).addClass("read");

            let mail = response;
            let role = mail.Sender.Role == null ? "" : `(${mail.Sender.Role})`;
            if (role !== 'Admin'){
                role = '';
            }
            let attach = mail.Attachments == null ? [] : mail.Attachments;
            var mydate = new Date(mail.CreateDate);
            var mailDate = dateFormat(mydate, "dd/mm/yyyy HH:MM")
            let str = `
                      <div class="flex head-message">
                        <div class="col-8 no-pad">
                            <div class="row">
                                <div class="col-2"><strong>${layoutText.Message_txt_from_whenclicked}</strong></div>
                                <div class="col-10 no-pad">
                                    <h3>: ${mail.Sender.Name} <span>${role}</span></h3>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2"><strong>${layoutText.Message_txt_title_whenclicked}</strong></div>
                                <div class="col-10 no-pad">
                                    <h3>: ${mail.Title}</h3>
                                </div>
                            </div>
                        </div>
                        <div class="col-4 head-right">
                            <div class="row">
                                <div class="col-4"><strong>${layoutText.Message_txt_date_whenclicked}</strong> :</div>
                                <div class="col-8 no-pad">
                                    <span class="date">${mailDate}</span>
                                </div>
                            </div>
                            <div class="tools">
                                <a href="#" onclick="forwardMail(911763610548942)"><i class="fa fa-forward" aria-hidden="true"></i> ${layoutText.Message_txt_forward_whenclicked}</a>
                                <a href="#" onclick="ReplyMail(911763610548942)"><i class="fa fa-reply" aria-hidden="true"></i> ${layoutText.Message_txt_replay_whenclicked}</a>
                            </div>
                        </div>
                    </div>

                    <div class="message-inbox">
                        ${mail.Body}
                    </div>
                    ${attach.map(a => `<div class="attachment" onclick="downloadAttachment(${a.Uid})"><i class="fa fa-paperclip" aria-hidden="true"></i>${a.Filename}</div><br/>`).join("")}
                    `;

            $("#read-mail").html(str);
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

function forwardMail(uid) {
    let url = `/message/createMessage?uid=${uid}&act=forward`;
    location.href = url;
}

function ReplyMail(uid) {
    let url = `/message/createMessage?uid=${uid}&act=reply`;
    location.href = url;
}

function downloadAttachment(uid) {
    location.href = `/message/downloadattachment?uid=${uid}`;
}