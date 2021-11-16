$(document).ready(function () {
    if ($('#ContentPlaceHolder1_btnExportarAExcel').hasClass('aspNetDisabled')) {
        $('#ContentPlaceHolder1_btnExportarAExcel').addClass('btn');
        $('#ContentPlaceHolder1_btnExportarAExcel').addClass('botonExportarExcel');
        $('#ContentPlaceHolder1_btnExportarAExcel').addClass('disabled');
    }


    $(".oculto").hide();


    var resultado = $('#ContentPlaceHolder1_hfErrorBusqueda');
    var mensaje = resultado.attr("value");
    if (mensaje != undefined) {
        resultado.attr("value", "");
        BootstrapDialog.show({
            title: 'Busqueda de Reservaciones...',
            cssClass: 'viva-dialog',
            message: mensaje,
            buttons: [{
                label: 'Cerrar',
                cssClass: 'btn-success',
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    }


    $('[data-toggle="tooltip"]').tooltip()
    $('[rel=tooltip]').tooltip();

    //var isPostback = $("#<%=hdnPostback.ClientID%>").val().toLowerCase() === "true";
    //if (isPostback == true) {
    //    $(".oculto").show();
    //}
    //else {
    //    $(".oculto").hide();
    //}

    //$('#<%=chkSoyExtranjero.ClientID%>').click(function () {
    //    var nodo = '#divExtranjero';
    //    if ($(nodo).is(":visible")) {
    //        $(nodo).hide();
    //    } else {
    //        $(".oculto2").hide("slow");
    //        $(nodo).fadeToggle("fast");
    //    }
    //});
});

$(".btn").click(function (event) {
    MostrarEnProceso();
});

$('button').click(function () {
    var toAdd = $("input[name=message]").val();
    $('#messages').append("<p>" + toAdd + "</p>");
});

$("#ContentPlaceHolder1_gvListaReservaciones_chkSeleccion").click(function (event) {
    MostrarEnProceso();
});

$("#rdbNacional").click(function () {
    alert('seleciono rdbNacional');

});


$(function () {
    $("#ContentPlaceHolder1_txtFechaProcIni").datepicker({
        maxDate: "<%=DateTime.Now.ToShortDateString()%>",
        format: "dd/mm/yyyy",
        todayBtn: "linked",
        language: "es",
        calendarWeeks: true,
        autoclose: true,
        toggleActive: true
    }).val();
});

$(function () {
    $("#ContentPlaceHolder1_txtFechaProcFin").datepicker({
        maxDate: "<%=DateTime.Now.ToShortDateString()%>",
        format: "dd/mm/yyyy",
        todayBtn: "linked",
        language: "es",
        calendarWeeks: true,
        autoclose: true,
        toggleActive: true
    }).val();
});


$(function () {
    $("#ContentPlaceHolder1_txtPNR").keypress(function (evt) {
        var charCode = evt.which || evt.keyCode;
        var str = String.fromCharCode(charCode);
        if (!/[0-9A-Za-z]/.test(str)) {
            evt.preventDefault();
        };
    });


});

$("#ContentPlaceHolder1_pnlGrid").scroll(function (event) {

    document.getElementById('ContentPlaceHolder1_pnlEnc').scrollLeft = document.getElementById('ContentPlaceHolder1_pnlGrid').scrollLeft
});

function MostrarEnProceso() {
    $("#cargando").css("display", "block");
}


function cambiarExtra(elinput) {
    if (elinput.checked == true) {
        $('#<%=txtRFC.ClientID%>').val('XEXX010101000');
        $('#<%=pnlTaxId.ClientID%>').attr("display", "block");
        $('#<%=pnlPaisRes.ClientID%>').attr("display", "block");
        $('#<%=txtRFC.ClientID%>').attr("ReadOnly", "True");
    }
    else {
        $('#<%=txtRFC.ClientID%>').val('');
        $('#<%=txtRFC.ClientID%>').removeAttr("ReadOnly");
        $('#<%=pnlTaxId.ClientID%>').attr("display", "none");
        $('#<%=pnlPaisRes.ClientID%>').attr("display", "none");
    }
}

function cambiarNacional(elinput) {
    if (elinput.checked == true) {
        $('#<%=txtRFC.ClientID%>').val('');
        $('#<%=txtRFC.ClientID%>').removeAttr("ReadOnly");
        $('#<%=pnlTaxId.ClientID%>').attr("display", "none");
        $('#<%=pnlPaisRes.ClientID%>').attr("display", "none");
    }
    else {
        $('#<%=txtRFC.ClientID%>').val('XEXX010101000');
        $('#<%=pnlTaxId.ClientID%>').attr("display", "block");
        $('#<%=pnlPaisRes.ClientID%>').attr("display", "block");
        $('#<%=txtRFC.ClientID%>').attr("ReadOnly", "True");
    }
}

function openModal() {
    document.getElementById('modal').style.display = 'block';
    document.getElementById('fade').style.display = 'block';
}

function closeModal() {
    document.getElementById('modal').style.display = 'none';
    document.getElementById('fade').style.display = 'none';
}

function FactConfirmation() {
    return confirm("¿Está seguro de continuar con el proceso de facturación con la información ingresada?");
}



//function ConfigureValidators() {
//    if (typeof Page_Validators != 'undefined') {
//        for (i = 0; i <= Page_Validators.length; i++) {
//            if (Page_Validators[i] != null) {
//                var visible = $('#' + Page_Validators[i].controltovalidate).parent().is(':visible');
//                Page_Validators[i].enabled = visible;
//            }
//        }
//    };
//}