var modal;
var instModal;

document.addEventListener('DOMContentLoaded', function() {
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
    
  });

//  $(document).ready(function(){
      function onBlazorReady() {

    $('.modal').modal();

    $('input[name="tipo-gestacao"]').change(function(){
        if($("input[name='tipo-gestacao']:checked").val() == 'multipla'){
            $('#numero-bebe').prop( "disabled", false );
        }
        else {
            $('#numero-bebe').prop( "disabled", true );
            $('#numero-bebe').val('');
        }
    });

    $('#editar-gestacao').on('click', function(){
        $('#editar-gestacao').attr('disabled','disabled');
        $("input[name='tipo-gestacao']").prop( "disabled", false );
        if($("input[name='tipo-gestacao']:checked").val() == 'multipla'){
            $('#numero-bebe').prop( "disabled", false );
        }
        else {
            $('#numero-bebe').prop( "disabled", true );
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
      $('#cpn').mask('00000000000000');
    
    
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

  
  