// JavaScript 
jQuery(document).ready(function(){
    jQuery('.wp-megamenu-main-wrapper.wpmm-orientation-horizontal.wpmm-onhover ul.wpmm-mega-wrapper > li > a').on('focusin', function(){
        jQuery(this).triggerHandler('focus');
        jQuery(this).closest('li').addClass('abrir');
    });
    jQuery('.wp-megamenu-main-wrapper.wpmm-orientation-horizontal.wpmm-onhover ul.wpmm-mega-wrapper > li > a').on('focusout', function(){
        jQuery(this).closest('li').removeClass('abrir');
    });
});

// Cria os Cookies
if (jQuery.cookie('contraste1') === "true") {
    jQuery('body').addClass('contraste1');
    jQuery('#erroIcon').attr('src', '/img/erro-n.png');
    jQuery('#sucessoIcon').attr('src', '/img/sucesso-n.png');
}

if (jQuery.cookie('contraste2') === "true") {
    jQuery('body').addClass('contraste2');
}

if (jQuery.cookie('resetCookie') === "true") {
    jQuery('body').removeClass('contraste1');
    jQuery('body').removeClass('contraste2');
    jQuery('#erroIcon').attr('src', '/img/erro.png');
    jQuery('#sucessoIcon').attr('src', '/img/sucesso.png');
}

// Acoes nos botoes de acessibilidade
jQuery(".btn-contraste1").click(function () {
    jQuery.removeCookie('contraste2', { path: '/' });
    jQuery.removeCookie('resetCookie', { path: '/' });
    if (!(jQuery.cookie('contraste1') === "true")) {
        jQuery('body').removeClass('contraste2');
        jQuery('body').addClass('contraste1');
        jQuery('#erroIcon').attr('src', '/img/erro-n.png');
        jQuery('#sucessoIcon').attr('src', '/img/sucesso-n.png');
        jQuery.cookie('contraste1', 'true', { path: '/' });
    }
    else {
        jQuery.cookie('contraste1', 'false', { path: '/' });
    }
    return false;
});

jQuery(".btn-contraste2").click(function () {
    jQuery.removeCookie('contraste1', { path: '/' });
    jQuery.removeCookie('resetCookie', { path: '/' });
    if (!(jQuery.cookie('contraste2') === "true")) {
        jQuery('body').removeClass('contraste1');
        jQuery('body').addClass('contraste2');
        jQuery.cookie('contraste2', 'true', { path: '/' });
    }
    else {
        jQuery.cookie('contraste2', 'false', { path: '/' });
    }
    return false;
});

jQuery(".btn-libras").click(function () {

    if (jQuery(this).attr('id') == 'ativo') {
        jQuery('.contEsq .divLibras').removeClass('abrevideo');
        jQuery('a.btn-libras').removeClass('ativo');
        jQuery('a.btn-libras').attr('id', '');

    } else {
        jQuery('a.btn-libras').attr('id', 'ativo');
        jQuery('.contEsq .divLibras').addClass('abrevideo');
        jQuery('a.btn-libras').addClass('ativo');
    }
    jQuery.cookie('librasCookie', jQuery(this).attr('id'), { path: '/' });
});

jQuery(".fontResizer_add").click(function () {
    maisFont();
});
jQuery(".fontResizer_minus").click(function () {
    menosFont();
});
jQuery(".fontResizer_reset").click(function () {
    resetFont();
});

// --->
// onBlazorAcessibilidadeReady()  -- INICIO 
// --->
function onBlazorAcessibilidadeReady() {

    jQuery(".fontResizer_add").click(function () {
        maisFont();
    });
    jQuery(".fontResizer_minus").click(function () {
        menosFont();
    });
    jQuery(".fontResizer_reset").click(function () {
        resetFont();
    });

    jQuery(".btn-contraste1").click(function () {
        jQuery.removeCookie('contraste2', { path: '/' });
        jQuery.removeCookie('resetCookie', { path: '/' });
        if (!(jQuery.cookie('contraste1') === "true")) {
            jQuery('body').removeClass('contraste2');
            jQuery('body').addClass('contraste1');
            jQuery('#erroIcon').attr('src', '/img/erro-n.png');
            jQuery('#sucessoIcon').attr('src', '/img/sucesso-n.png');
            jQuery.cookie('contraste1', 'true', { path: '/' });
        }
        else {
            jQuery.cookie('contraste1', 'false', { path: '/' });
        }
        return false;
    });

    jQuery(".btn-contraste2").click(function () {
        jQuery.removeCookie('contraste1', { path: '/' });
        jQuery.removeCookie('resetCookie', { path: '/' });
        if (!(jQuery.cookie('contraste2') === "true")) {
            jQuery('body').removeClass('contraste1');
            jQuery('body').addClass('contraste2');
            jQuery.cookie('contraste2', 'true', { path: '/' });
        }
        else {
            jQuery.cookie('contraste2', 'false', { path: '/' });
        }
        return false;
    });
}
// --->
// onBlazorAcessibilidadeReady()  -- FIM 
// --->

(function () {
    var tam = jQuery.cookie('tamanhoLetra');
    jQuery('html').css("font-size", tam + "px");
})();

function maisFont() {
    var tamanho = jQuery('html').css("font-size");
    var maisUm = parseInt(tamanho.substr(0, 2));
    maisUm++;
    jQuery.cookie('tamanhoLetra', maisUm, { path: '/' });
    jQuery('html').css("font-size", maisUm + "px");
    //console.log("MAIS", jQuery.cookie('tamanhoLetra'));
}
function menosFont() {
    var tamanho = jQuery('html').css("font-size");
    var maisUm = parseInt(tamanho.substr(0, 2));
    maisUm--;
    jQuery.cookie('tamanhoLetra', maisUm, { path: '/' });
    jQuery('html').css("font-size", maisUm + "px");
    //console.log("MENOS", jQuery.cookie('tamanhoLetra'));
}
function resetFont() {
    jQuery('html').css("font-size", "14px");

    jQuery.removeCookie('contraste2', { path: '/' });
    jQuery.removeCookie('contraste1', { path: '/' });

    if (!(jQuery.cookie('resetCookie') === "true")) {
        jQuery('body').removeClass('contraste1');
        jQuery('body').removeClass('contraste2');
        jQuery('#erroIcon').attr('src', '/img/erro.png');
        jQuery('#sucessoIcon').attr('src', '/img/sucesso.png');
        jQuery.cookie('resetCookie', 'true', { path: '/' });

        jQuery('.fontSize').removeAttr('style');
        jQuery('.fontSize15').removeAttr('style');
        jQuery.removeCookie('tamanhoLetra', { path: '/' });
    }
    else {
        jQuery('.fontSize').removeAttr('style');
        jQuery('.fontSize15').removeAttr('style');
        jQuery.removeCookie('tamanhoLetra', { path: '/' });
        jQuery.cookie('resetCookie', 'false', { path: '/' });
    }
}

// APARECE O MENU

jQuery(".menu-acess").focusin(function () {
    jQuery(".menu-acess").css("top", "38px");
    jQuery(".menu-acess").css("left", "15.5%");
});
jQuery(".menu-acess").focusout(function () {
    jQuery(".menu-acess").css("top", "-1300px");
});

