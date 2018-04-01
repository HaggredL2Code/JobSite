
function check(listItemsArray, toAddItem) {
    existed = false
    for (var i = 0; i < listItemsArray.length; i++) {
        if (parseInt(toAddItem) == parseInt(listItemsArray[i])) {
            existed = true;
            break;
        }
    }
    return existed
}