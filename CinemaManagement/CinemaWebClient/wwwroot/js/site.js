
//configuration toastr alter
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function NotificationMessageSuccess(message) {
    toastr.success(message, "Notification");
}
function NotificationMessageError(message) {
    toastr.error(message, "Notification");
}


function formatStringToCurrencyVND(number) {
    var length = number.length;
    var count = 0;
    var result = "";
    const chars = number.split('');
    if (length % 3 === 0) {
        count = -1;
    } else {
        count = 0;
    }
    for (let i = length - 1; i >= 0; i--) {
        if ((i + 1) % 3 === 0) {
            count++
        }
    }
    var result = number.split("");
    var mod = number.length % 3;
    var positions = [];
    for (let i = 0; i <= count; i++) {
        if (mod % 3 === 0) {
            var position = 0;
            if (i === 0) continue;
            if (i === 1) {
                position = i * 3;
            } else {
                position = i * 3 + i - 1;
            }
            positions.push(position);
        } else {
            if (i === count) continue;
            var position = 0;
            if (i === 0) {
                position = i * 3 + mod
            } else {
                position = i * 3 + mod + i
            }
            positions.push(position);
        }
    }
    for (let i = 0; i < positions.length; i++) {
        var array = [","];
        result = result.concat(array);
        if (mod % 3 === 0) {
            var temp = result[positions[i]];
            for (let j = result.length - 1; j >= 0; j--) {
                if (j >= positions[i]) {
                    result[j] = result[j - 1]
                }
            }
            result[positions[i] - 1] = temp;
            result[positions[i]] = ",";
        } else {
            var temp = result[0];
            for (let j = result.length - 1; j > mod; j--) {
                if (j >= positions[i]) {
                    result[j] = result[j - 1]
                }
            }

            result[positions[i]] = ",";
        }


    }
    var x = "";
    for (let i = 0; i < result.length; i++) {
        x += result[i];
    }
    return x;
}

function MakeQueryEqual(field, value, isFirst) {
    if (isFirst) return `$filter=${field} eq ${value}`;
    return ` and ${field} eq ${value}`
}

function MakeQueryConditionContainString(field, value, isFirst) {
    if (isFirst) {
        var query = "$filter=contains(" + field + ", '" + value + "') eq true";
    } else {
        var query = " and contains(" + field + ", '" + value + "') eq true";
    }

    return query;
}

function MakeQueryConditionContainStringCastingNumToString(field, value, isFirst) {
    if (isFirst) {
        var query = "$filter=contains(cast(" + field + ",Edm.String),'" + value + "') eq true";
    } else {
        var query = " and contains(cast(" + field + ",Edm.String),'" + value + "') eq true";
    }
    return query;
}

function ParseDateTime(inputDate) {
    const date = new Date(inputDate);

    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    const seconds = String(date.getSeconds()).padStart(2, "0");

    const formattedDate = `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
    return formattedDate;
}
function CompareValueGreaterDateNow(value) {
    const dateNow = new Date();
    const dateValue = new Date(value);
    if (dateValue > dateNow) {
        return true;
    }
    return false;
}