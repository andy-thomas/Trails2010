
function InitPersonIndexForm() {
    $('.personcreateform').hide();

}


function InitNewPersonForm(caller) {
    $(caller).toggle();
    $('.personcreateform input:text').val('');
    //alert($('.personcreateform input:text').parent().html());
    $('.personcreateform').show();
}

function HideNewPersonForm() {
    var isValid = $("#IsModelValid")[0].value
    if (isValid == "True") {
        $('.personcreateform').hide();
        $('#shownewpersonform').show();
    }
    else {
        $('.personcreateform').show();
        $('#shownewpersonform').hide();
    }
}

function InitEditPersonForm(caller, personId) {
    // Find the parent div with the 'data-row' class
    // (i.e. the read-only view of the Person data)
    // and hide it  
    $(caller).parents('.data-row').hide();

    // Show the view which displays the editable version of the data
    $('#datarowedit' + personId).show();

    // Show the form inside the view
    $('.personeditform').show();
}



