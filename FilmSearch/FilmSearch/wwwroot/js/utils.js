function isEmptyString(str) {
    return str == null || str === "";
}

function requireNonNull(str, callback) {
    if (isEmptyString(str)) {
        callback();
        return true;
    }

    return false;
}

function convertToDefaultDateFormat(dateStr) {
    if (isEmptyString(dateStr)) return null;

    var date = new Date(dateStr);
    return ('0' + (date.getMonth() + 1)).slice(-2) +
        '/' +
        ('0' + date.getDate()).slice(-2) +
        '/' +
        date.getFullYear();
}

function convertFromDefaultDateFormat(dateStr) {
    if (isEmptyString(dateStr)) return null;

    var date = new Date(dateStr);
    return date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) +
        '-' +
        ('0' + date.getDate()).slice(-2);
}