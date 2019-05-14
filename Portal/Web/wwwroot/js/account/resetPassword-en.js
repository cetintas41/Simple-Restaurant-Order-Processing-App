function resetPassword(btn) {
    var passRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");

    var Email = $('#email').val();
    var Token = $('#token').val();
    var NewPassword = $('#password').val();
    var NewPasswordAgain = $('#password-again').val();

    if (!passRegex.test(NewPassword)) {
        toastr.error('Invalid password');
        return;
    }

    if (Email === "") {
        toastr.error('Invalid e-mail');
        return;
    }

    if (Token === "") {
        toastr.error('Invalid token');
        return;
    }

    if (NewPassword !== NewPasswordAgain) {
        toastr.error('Incompatible passwords');
        return;
    }

    $(btn).html('Please wait...');

    $.post({
        url: 'Account/ResetPassword',
        data: { Email, Token, NewPassword, NewPasswordAgain },
        complete: function (xhr) {
            if (xhr.status === 200) {
                toastr.options.onHidden = function () { window.location.href = "https://wwww.google.com.tr" };
                toastr.success(xhr.responseText);
            }
        },
        error: function (xhr) {
            if (xhr.status === 400) { toastr.error(xhr.responseText); }
        }
    });
}