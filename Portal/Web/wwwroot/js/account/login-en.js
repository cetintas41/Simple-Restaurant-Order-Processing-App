$(function () { $('.selectpicker').selectpicker(); });

$('#selectpicker-culture').change(function () {
    $('#input-culture').val($('#selectpicker-culture').val());
    $('#input-returnurl').val(window.location.pathname + window.location.search);
    $('#form-change-language').submit();
});

function forgotPassword() {
    var emailRegex = new RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
    var fpmail = $('#forgot-pass').val();

    if (!emailRegex.test(fpmail)) {
        toastr.error('Invalid e-mail');
        return;
    }

    $.post({
        url: 'Account/ForgotPassword',
        data: { email: fpmail },
        complete: function (xhr) {
            if (xhr.status === 200) {
                //toastr.options.onHidden = function () { window.location.reload(); };
                $('#modal-forgot-password').modal('toggle');
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            $('#modal-forgot-password').modal('toggle');
            if (xhr.status === 400) { toastr.error(xhr.responseText); }
        }
    });
}

function login(btn) {
    var emailRegex = new RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
    var passRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");

    if (!emailRegex.test($('#login-email').val())) {
        toastr.error('Invalid e-mail');
    }
    else if (!passRegex.test($('#login-password').val())) {
        toastr.error('Invalid password');
    }
    else {
        $(btn).html('Logging...');
        document.getElementById('form-login').submit();
        //$('#form-login')[0].submit();
    }
}

