/* Popup */

function showPopupError() {
    $('#popupErrorRequest').modal('show');
}


// Progress Bar
function openProcess() {    
    $('#popup-process').show();   
}
function hideProcess() {    
    $('#popup-process').hide();    
}

// Knockout
// First: Create config
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null,
    decorateElement: true,
    errorClass: "error",
    errorMessageClass: "error-desc"
});
var mustEqual = function (val, other) {
    return val == other();
};


function formatInputToMoney(value) {
    value += '';
    value = value.replace(/[~!#$%^&*()_+=<>?]/g, '');
    value = value.replace(/[a-z]/g, '');
    value = value.replace(/[A-Z]/g, '');
    value = value.replace(/\,/g, '');
    value = value.replace(/\,/g, '');
    value = value.replace(/\,/g, '');
    value = value.replace(/\,/g, '');
    value = value.replace(/\-/, '');
    value = value.replace(/\-/, '');
    value = value.replace(/\-/, '');
    value = value.replace(/\-/, '');
    value = value.replace(/\./, '');
    value = value.replace(/\./, '');
    value = value.replace(/\./, '');
    value = value.replace(/\./, '');
    if (value === '')
        return value;
    return parseFloat(value).format(0, 3, ',');
}

// Apps
$(document).ready(function () {
    

});
