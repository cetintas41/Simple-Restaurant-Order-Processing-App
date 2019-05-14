function resetPassword(btn) {
    var passRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");

    var Email = $('#email').val();
    var Token = $('#token').val();
    var NewPassword = $('#password').val();
    var NewPasswordAgain = $('#password-again').val();

    if (!passRegex.test(NewPassword)) {
        toastr.error('Geçersiz şifre');
        return;
    }

    if (Email === "") {
        toastr.error('Geçersiz e-posta');
        return;
    }

    if (Token === "") {
        toastr.error('Geçersiz anahtar');
        return;
    }

    if (NewPassword !== NewPasswordAgain) {
        toastr.error('Şifreler uyuşmuyor');
        return;
    }

    $(btn).html('İşleminiz Yapılıyor...');

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