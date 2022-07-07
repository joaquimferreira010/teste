var modal;
var instModal;
function onBlazorDatePicker() {
   // document.addEventListener('DOMContentLoaded', function () {
        var datepicker_pt_br = {
            today: 'Hoje',
            clear: 'Limpar',
            done: 'Ok',
            cancel: 'Cancelar',
            nextMonth: 'Próximo mês',
            previousMonth: 'Mês anterior',
            weekdaysAbbrev: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
            weekdaysShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
            weekdays: ['Domingo', 'Segunda-Feira', 'Terça-Feira', 'Quarta-Feira', 'Quinta-Feira', 'Sexta-Feira', 'Sábado'],
            monthsShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        }
        var options = {
            container: 'body',
            format: 'dd/mm/yyyy',
            minDate: new Date(),
            showDaysInNextAndPreviousMonths: true,
            i18n: datepicker_pt_br,
            //outras configurações
        }
        var elems = document.querySelectorAll('.datepicker');
        var instances = M.Datepicker.init(elems, options);

    //});
}
//  $(document).ready(function(){
function onBlazorReady() {

    $('.modal').modal(); 

    $('input[name="tipo-gestacao"]').change(function(){
        if($("input[name='tipo-gestacao']:checked").val() == 'multipla'){
            $('#numero-bebe').prop("disabled", false);
            $('#numero-bebe').mask('0');
            $('#numero-bebe').focus();
        }
        else {
            $('#numero-bebe').prop("disabled", true );
            $('#numero-bebe').val('');
        }
    });

    $('#editar-gestacao').on('click', function(){
        $('#editar-gestacao').attr('disabled','disabled');
        $("input[name='tipo-gestacao']").prop("disabled", false );
        if($("input[name='tipo-gestacao']:checked").val() == 'multipla'){
            $('#numero-bebe').prop("disabled", false);
            $('#numero-bebe').mask('0');
        }
        else {
            $('#numero-bebe').prop("disabled", true );
        }

    });

    $('#editar-cns').on('click', function(){
        $(this).parents('.protocolo-item').find('.form-item').removeClass('readonly');
        $(this).parents('.protocolo-item').find('.cns-crianca').prop("readonly", false );
        $(this).parents('.protocolo-item').find('.cns-crianca').attr('aria-readonly',false);
        $(this).parents('.protocolo-item').find('.cns-crianca').focus();
    });

    $('#cns').mask('000 0000 0000 0000');
    $('.cns-crianca').mask('000 0000 0000 0000');
    $('#cpn').mask('000000000000000');
    $('#numero-bebe').mask('0');
    $('#data-matricula').mask('00/00/0000');

          $(".datepicker").datepicker();
          //var diaSemana = ['Domingo', 'Segunda-Feira', 'Terca-Feira', 'Quarta-Feira', 'Quinta-Feira', 'Sexta-Feira', 'Sabado'];
          //var mesAno = ['Janeiro', 'Fevereiro', 'Marco', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
          //var data = new Date();
          //var hoje = diaSemana[data.getDay()] + ', ' + mesAno[data.getMonth()] + ' de ' + data.getFullYear();
          ////$("#dataPesquisa").attr("value", hoje);
          //$(".datepicker").pickadate({
          //    monthsFull: mesAno,
          //    monthsShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
          //    weekdaysFull: diaSemana,
          //    weekdaysShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
          //    weekdaysLetter: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
          //    selectMonths: true,
          //    selectYears: true,
          //    clear: false,
          //    format: 'dddd/mm/yyyy',
          //    today: "Hoje",
          //    close: "X",
          //    min: new Date(data.getFullYear() - 1, 0, 1),
          //    max: new Date(data.getFullYear() + 1, 11, 31),
          //    closeOnSelect: true
          //});    
          //$("#data-matricula").click(function (event) {
          //    event.stopPropagation();
          //    $(".datepicker").first().pickadate("picker").open();
          //});

//para deixar o item de menu ativo - tarja
          $('ul.right li').on('click', function () {
              var clicked = $(this);
              $('ul.right li').each(function () {
                  if ($(this).hasClass('active')) {
                      $(this).removeClass('active');
                  }
              });
              $(this).addClass('active');
          });

    $("#playCaptcha").keyup(function (event) {
        var keyCode = (event.keyCode ? event.keyCode : event.which);
        // Enter
        if (keyCode === 13) {
            event.preventDefault();
            $("#playCaptcha").click();
        }
    });

    //$("#gerarCaptcha").keyup(function (event) {
    //    var keyCode = (event.keyCode ? event.keyCode : event.which);
    //    // Enter
    //    if (keyCode === 13) {
    //        event.preventDefault();
    //        $("#gerarCaptcha").click();
    //    }
    //});

    $("#playCaptcha").click(function () {
        playCaptcha();
    });

    if ($("#data-matricula").prop("disabled") == false) {
        $("#data-matricula").focus();
    }

    $("#btn-ver_aviso").click(function () {
        $('#aviso-modal').modal('open');
    });

    //$("#btn_abrir_menu").click(function () {
    //    openMenu();
    //});
   
};
//);

  function verificardados(dado) {
    if(dado == '2') {confirm('Confirmar solicitação da vaga?');}
  }

  function openMenu() {
        $('.btn-menu').toggleClass('menushow');
        $('.sidemenu').toggleClass('menushow');
  }

  function verificaCNS(dado) {
    $('#cns-crianca-modal-1').modal('open');
  }

  function cancelarCNS(dado) {
    
  }

  function confirmarCNS(dado) {
    
  }

  
//JS Adicionados
function playCaptcha() {    
    var player = document.getElementById("player");
    if (player.paused) {
        $('#textoCaptcha').focus();
        player.play();        
    } else {
        player.pause();
    }
}

window.funcoes = {
    alerta: function (mensagem) {
        alert(mensagem);
    },
    imprimir: function () {
        window.print();
    },
    ativarNavItemMenu: function (item) {
        $('ul.right li').each(function () {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
            }
        });
        if ($("#" + item) != undefined) {
            $("#" + item).addClass('active');
        }
    },
    abrirModal: function(modal_id) {
        $("#" + modal_id).modal('open');        
    },
    abrirModalEstatico: function (modal_id) {
        $("#" + modal_id).modal({
            dismissible: false
        });
        $("#" + modal_id).modal('open');
    },
    abrirModalMobile: function (modal_id) {
        if (screen.width < 640 || screen.height < 480) {//mobile
            $('.modal').modal();
            $("#" + modal_id).modal('open');
        }
        
    },
    focusElement: function(campo_focus) {
        var element = window.document.getElementById(campo_focus); 
        if (element != null && element != undefined) {
            element.focus();
        }

       // if ($("#" + campo_focus) != undefined) {
       //     $("#" + campo_focus).prop('disabled', false);  
       //     $("#" + campo_focus).focus();
       //}
        
    },
    abrirModalMensagem: function (mensagem) {
        AbrirModalErro(mensagem);
    },
    abrirModalMensagemFocus: function (mensagem, campo_focus) {
        AbrirModalErro(mensagem, function () {
                $("#" + campo_focus).focus();
        });
    },
    openMenu: function () {
        $('.btn-menu').toggleClass('menushow');
        $('.sidemenu').toggleClass('menushow');
    }

}
function AbrirModalErro(mensagem, funcaoRetorno) {

    var modalFracasso = $("#modalFracasso");

    $(modalFracasso).modal({
        onOpenEnd: function () {
            $(btnFecharModalErro).focus();
        },
        onCloseEnd: function () {
            $('body').css('overflow', '') /* O modal tira o scroll do body e em alguns casos não coloca de volta */
        }
    });

    $(modalFracasso).find("#contentModalErro").empty();
//  $(modalFracasso).find("#contentModalErro").append("<p>" + mensagem + "<p>");
    $(modalFracasso).find("#contentModalErro").append("<br>" + mensagem + "<br>");

    $(modalFracasso).show();
    $(modalFracasso).modal('open');

    var btnFecharModalErro = $(modalFracasso).find("#btnFecharModalErro");
    $(btnFecharModalErro).unbind("click");

    $(btnFecharModalErro).click(function () {
        var modalFracasso = $("#modalFracasso");
        $(modalFracasso).modal('close')
        if (funcaoRetorno != undefined) {
            funcaoRetorno();
        }
    });

}
function SetarFocus(campo) {
    window.document.getElementById(campo).focus();
}

function ativarMascarasCamposInformarNascimento() {
    $('.cns-crianca').mask('000 0000 0000 0000');
}

 //atualizar o title para cada pagina
setTitle = (title) => { document.title = "Mãe Paulistana  " + (title == ""? "" : (" - " + title)); };




