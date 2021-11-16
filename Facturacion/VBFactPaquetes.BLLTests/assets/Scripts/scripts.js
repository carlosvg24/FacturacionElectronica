

/*FUNCIÓN PARA VALIDAR SÓLO INGRESO DE MAYUSCULAS AL TECLEAR*/
function mayus(e) {
    e.value = e.value.toUpperCase();
}

/*Función que permite Números y Letras*/
function SoloNumeroLetras() {
    if ((event.keyCode < 48) || (event.keyCode > 57) && (event.keyCode < 65) || (event.keyCode > 90) && (event.keyCode < 97) || (event.keyCode > 122) && (event.keyCode != 209) && (event.keyCode != 241))
        event.returnValue = false;
}


/*Función que permite Validar RFC con ampersand ejemplo (X&Y081118ICA)*/
//function validarRFC() {
//    if ((event.keyCode != 38) && (event.keyCode < 48) || (event.keyCode > 57) && (event.keyCode < 65) || (event.keyCode > 90) && (event.keyCode < 97) || (event.keyCode > 122) && (event.keyCode != 209) && (event.keyCode != 241))
//        event.returnValue = false;
//}


