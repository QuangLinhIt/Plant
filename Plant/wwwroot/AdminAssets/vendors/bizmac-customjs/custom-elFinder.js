
function InitTinymce(divId) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }

    return  tinymce.init({
        selector: `#${divId}`,
        height: 700,
        menubar: false, 
        branding: false,
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak textcolor link image ',
            'searchreplace searchreplace  visualchars code fullscreen',
            'insertdatetime emoticons directionality media table contextmenu paste code help wordcount filemanager responsivefilemanager media quickbars'
        ],

        contextmenu: "paste | link image inserttable responsivefilemanager | cell row column deletetable",
        toolbar: '| undo redo | bold italic underline strikethrough | fontselect formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | table responsivefilemanager | preview code | media | removeformat |  help',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }

    });
}

