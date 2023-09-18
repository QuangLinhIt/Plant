
$(document).ready(function () {

    //setTimeout(function () {
    //    $('[data-toggle="tooltip"]').tooltip();


    //}, 1000);


});
function GetListItemChecked(name) {
    var listCheckbox = document.getElementsByName(name);
    var arrItem = [];
    for (var i = 0; i < listCheckbox.length; i++) {
        if (listCheckbox[i].checked) {
            arrItem.push(listCheckbox[i].value);
        }
    }
    return arrItem;
}
function KtCheckbox() {

    var listCheckbox = document.getElementsByName("Chk");
    var kt = true;
    for (var i = 0; i < listCheckbox.length; i++) {
        if (!listCheckbox[i].checked) {
             kt = false; break;
        }
    }
    var chk = $("#customcheck-all");
    chk[0].checked = kt;
}
function AlertToast(title, content, type) {
    $.toast({
        heading: title + "!",
        text: `<p>${content}</p>`,
        position: 'top-right',
        loaderBg: '#7a5449',
        class: `jq-toast-${type}`,
        hideAfter: 3500,
        stack: 6,
        showHideTransition: 'fade'
    });
}
function CheckAll() {
    // debugger;
    var chk = $("#customcheck-all");
    chk[0].checked;
    var listCheckbox = document.getElementsByName("Chk");

    for (var i = 0; i < listCheckbox.length; i++) {
        if (listCheckbox[i].type === "checkbox") {
            listCheckbox[i].checked = chk[0].checked;;
        }
    }
}

function SweetAlert(title, content, type) {
    Swal.fire({
        title: title,
        text: content,
        type: type,
        showCancelButton: false,
    })
}

function ConvertBoolToInt(value) {
    if (value) {
        return 1;
    }
    return 0;

}
function ConvertIntToBool(value) {
    if (value === "1") {
        return true;
    } else if (value === "0") {
        return false;
    }
    return null;

}

function ConvertNumberToMoney(id, number) {
    try {
        if (number == "") {
            $("#" + id).val(0);

        } else {
            
            number = number.replace(/\,/g, '');
            Number.prototype.format = function (n, x) {
                var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
                return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,');
            };

            var rs = JSON.parse(number).format(0, 3);

            //var rs = formatter.format(number); 
            $("#" + id).val(rs);
        }
  
    } catch (e) {
        $("#" + id).val(number);

    }

}
function ConvertNumberToMoney2(number) {
    try {
        number = number.replace(/\,/g, '');
        Number.prototype.format = function (n, x) {
            var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
            return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,');
        };

        var rs = JSON.parse(number).format(0, 3);
        return rs;

    } catch (e) {
        return JSON.parse(0);
    }

}
function ConvertMoneyToNumber(money) {
    try {
        return money.replace(/\,/g, '');

    } catch (e) {
        0
    }
}

function InitDatePicker(id,value) {
    //$(id).datepicker({
    //    autoHide: true,
    //    zIndex: 2048

    //});
    $(id).datetimepicker({
        format: 'm/d/Y H:i',
        formatTime: 'H:i',
        formatDate: 'm/d/Y'
    });
    try {
        if (value === null || value === "") {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            $(id).val(mm + "/" + dd + "/" + yyyy + " " + today.getHours() + ":" + today.getMinutes());
        }
        else {
            $(id).val(moment(value).format("MM/DD/YYYY hh:mm"));


        }
    } catch (e) {
      var today = new Date();
      var dd = today.getDate();
      var mm = today.getMonth() + 1;
      var yyyy = today.getFullYear();
        $(id).val(mm + "/" + dd + "/" + yyyy + " " + today.getHours() + ":" + today.getHours() );
    }
    currentDataCommon.publishDate = $(id).val();
   
}

function dataURItoBlob(dataURI) {
   try {
                    var byteString;
                    if (dataURI.split(',')[0].indexOf('base64') >= 0)
                        byteString = atob(dataURI.split(',')[1]);
                    else
                        byteString = unescape(dataURI.split(',')[1]);

                    // separate out the mime component
                    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

                    // write the bytes of the string to a typed array
                    var ia = new Uint8Array(byteString.length);
                    for (var i = 0; i < byteString.length; i++) {
                        ia[i] = byteString.charCodeAt(i);
                    }

                    return new Blob([ia], {type:mimeString});
   }
   catch (e) {
       return "";
   }   // convert base64/URLEncoded data component to raw binary data held in a string
}
function ToolTip() {
    if ($('[data-toggle="tooltip"]').length > 0){
        $('[data-toggle="tooltip"]').tooltip({
                container: 'table',
        }).on('click', function () {
            $(this).tooltip('hide')
        });
    }


    //$('[data-toggle="tooltip"]').on('click', function () {
    //    $(this).tooltip('hide')
    //})
}


function compareValues(key, order = 'asc') {
    return function innerSort(a, b) {
        if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
            // property doesn't exist on either object
            return 0;
        }

        const varA = (typeof a[key] === 'string')
            ? a[key].toUpperCase() : a[key];
        const varB = (typeof b[key] === 'string')
            ? b[key].toUpperCase() : b[key];

        let comparison = 0;
        if (varA > varB) {
            comparison = 1;
        } else if (varA < varB) {
            comparison = -1;
        }
        return (
            (order === 'desc') ? (comparison * -1) : comparison
        );
    };
}

function unicodeBase64Encode(text) {
    try {
        return window.btoa(encodeURIComponent(text).replace(/%([0-9A-F]{2})/g, function (match, p1) {
            return String.fromCharCode('0x' + p1);
        }));
    }
    catch (e) {
        return;
    }
}

function unicodeBase64Decode(text) {
    try {
        return decodeURIComponent(Array.prototype.map.call(window.atob(text), function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }
    catch (e) {
        return;
    }
}
