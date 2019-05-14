function deleteFirmUser() {
    var userId = $('#delete-id').val();

    if (userId === "" || userId === null || typeof userId === "undefined") {
        toastr.error('Geçersiz kimlik');
        return;
    }

    $.post({
        url: 'Admin/DeleteFirmUser',
        data: { userId },
        complete: function (xhr) {
            if (xhr.status === 200) {
                toastr.options.onHidden = function () { window.location.reload(); };
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            if (xhr.status === 400) { toastr.error(xhr.responseText); }
        }
    });
}

function changeAccess(userId) {
    if (userId === "" || userId === null || typeof userId === "undefined") {
        toastr.error('Geçersiz kimlik');
        return;
    }

    $.post({
        url: 'Admin/ChangeAccess',
        data: { userId },
        complete: function (xhr) {
            if (xhr.status === 200) {
                //toastr.options.onHidden = function () { window.location.reload(); };
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            if (xhr.status === 400) { toastr.error(xhr.responseText); }
        }
    });
}

function createFirm() {
    var emailRegex = new RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
    var passRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");

    var Logo = $('#logo')[0].files[0];
    var Name = $('#firm-name').val();
    var PhoneNumber = $('#firm-tel').val();
    var Email = $('#firm-email').val();
    var Password = $('#firm-password').val();
    var PasswordConfirm = $('#firm-password-confirm').val();


    if (Logo === null) {
        toastr.error('Logo boş bırakılamaz');
        return;
    }

    if (Name === "") {
        toastr.error('Firma adı boş bırakılamaz');
        return;
    }

    if (Email === "") {
        toastr.error('Firma e-posta boş bırakılamaz');
        return;
    }


    if (Password === "") {
        toastr.error('Şifre boş bırakılamaz');
        return;
    }


    if (PasswordConfirm !== Password) {
        toastr.error('Şifreler uyuşmuyor');
        return;
    }

    if (PhoneNumber === "") {
        toastr.error('Telefon boş bırakılamaz');
        return;
    }


    if (!emailRegex.test(Email)) {
        toastr.error('Geçersiz e-posta');
        return;
    }

    if (!passRegex.test(Password)) {
        toastr.error('Geçersiz şifre');
        return;
    }

    var data = new FormData();
    data.append('Logo', Logo);
    data.append('Name', Name);
    data.append('PhoneNumber', PhoneNumber);
    data.append('Email', Email);
    data.append('Password', Password);
    data.append('PasswordConfirm', PasswordConfirm);

    jQuery.ajax({
        url: 'Admin/CreateFirm',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST',
        type: 'POST', // For jQuery < 1.9
        complete: function (xhr) {
            if (xhr.status === 200) {
                toastr.options.onHidden = function () { window.location.reload(); };
                $('#modal-create-firm').modal('toggle');
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            if (xhr.status === 400) {
                toastr.error(xhr.responseText);
            }
        }
    });
}

$(document).ready(function () {
    $('#modal-create-firm').on('hidden.bs.modal', function () {
        $('#country-list').empty();
        $('#city-list').empty();
    });

    $('#btn-logo').click(function () {
        $('#logo').click();
    });

    $('#logo').change(function () {
        var filename = $('#logo')[0].files[0].name;
        $('#btn-logo').html(filename);
    });
});

function changePage(p) {
    $('#p').val(p);
    $('#search-paging-form').submit();
}

function changeTop(t) {
    $('#t').val(t);
    $('#search-paging-form').submit();
}

function deleteItem(url) {
    var id = $('#delete-id').val();
    var row = $('#row-id').val();

    if (id === "" || id === null || typeof id === "undefined") {
        toastr.error('Geçersiz kimlik');
        return;
    }

    $.post({
        url: url,
        data: { id },
        complete: function (xhr) {
            if (xhr.status === 200) {
                //toastr.options.onHidden = function () { window.location.reload(); };
                $("#row_" + row).remove();
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            if (xhr.status === 400) { toastr.error(xhr.responseText); }
        }
    });
}