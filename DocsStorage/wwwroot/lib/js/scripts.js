$('#more').click(() => { loadItems() });

$('#form').submit(() => {
    let formData = new FormData($('#form')[0]);
    $.ajax({
        type: 'POST',
        enctype: 'multipart/form-data',
        url: 'http://localhost:57803/api/save',
        data: formData,
        contentType: false,
        processData: false,
        cache: false,
        success: () => { window.location.href = window.location.origin + '/home/index'; },
        error: (e) => {
            $("#btnSubmit").prop("disabled", false);
            $('#error').text('Произошла ошибка во время создания записи, попробуйте позже.');
            console.log('Error: ' + e.responseText);
        }
    });
    return false;
});

$("#findName").change(() => {
    let name = $("#findName").val();
    if (name == '')
        window.location.href = window.location.origin + '/home/index';
    else {
        $.ajax({
            type: 'GET',
            url: window.location.origin + '/home/getitemsbyname/?name=' + name,
            statusCode: {
                200: (data) => {
                    $('#tbody').empty();
                    $('#tbody').append(data);
                },
                500: () => { $('#result').attr('class', 'text-danger').text('Произошла ошибка, попробуйте позже!') }
            }
        });
    }
});

let page = 1;

function loadItems() {
    $.ajax({
        type: 'GET',
        url: window.location.origin + '/home/getitems/' + page,
        statusCode: {
            200: (data) => {
                $('#tbody').append(data);
                page++;
            },
            404: () => { $('#result').attr('class', 'text-info').text('Больше нет данных.') },
            500: () => { $('#result').attr('class', 'text-danger').text('Произошла ошибка, попробуйте позже!') }
        }
    });

};

function SetDocumentPropirties() {
    let fileInput = document.getElementById('DocumentData');

    if ('files' in fileInput) {
        if (fileInput.files.lenght != 0) {
            let fullFileName = fileInput.files[0].name;
            let mass = fullFileName.split('.');
            let extension = mass[mass.length - 1];
            let name = fullFileName.substring(0, (fullFileName.length - extension.length) - 1);

            $('#DocumentName').val(name);
            $('#Exstension').val(extension);
            $('#LoadDate').val(new Date().toLocaleString('ru'));
            $('#DocumentID').val(0);
        }
    }
}

function deleteDoc(id, documentName) {
    $.ajax({
        type: 'DELETE',
        url: 'http://localhost:57803/api/delete/' + id,
        contentType: false,
        processData: false,
        cache: false,
        statusCode: {
            200: () => { deleteRow(id, documentName); },
            500: () => { errorDelete(); }
        }
    });
};

function deleteRow(idRow, documentName) {
    var table = document.querySelector('tbody');
    var row = document.getElementById(idRow);
    table.removeChild(row);

    $('#result').attr('class', 'text-success').text('Документ ' + documentName + ' был успешно удален.').re;
};

function errorDelete() {
    $('#result').attr('class', 'text-danger').text('Произошла ошибка во время удаления документа. Попробуйте позже.');
};





