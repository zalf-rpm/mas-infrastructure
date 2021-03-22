function saveAsBase64(fileName, byteBase64) {
    var link = document.createElement("a");
    link.download = fileName;
    link.href = "data:application/csv;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
