mergeInto(LibraryManager.library, {
  UnzipInWebGL: function (zipDataPtr, gameTypePtr, fileNamePtr) {
    var zipData = new Uint8Array(
      Module.HEAPU8.buffer,
      zipDataPtr,
      arguments[arguments.length - 3]
    );
    var gameType = UTF8ToString(gameTypePtr);
    var fileName = UTF8ToString(fileNamePtr);

    var zip = new JSZip();
    zip
      .loadAsync(zipData)
      .then(function (zip) {
        zip.forEach(function (relativePath, file) {
          file.async("blob").then(function (blob) {
            // Store the extracted file in browser storage or perform your action
            console.log("Extracted file: " + relativePath);
          });
        });
      })
      .catch(function (e) {
        console.error("Error unzipping file: ", e);
      });
  },
});
