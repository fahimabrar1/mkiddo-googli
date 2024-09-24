// Save this as Assets/Plugins/WebGLEncryptedStorage.jslib

var WebGLEncryptedStorage = {
  SaveEncryptedData: function (key, encryptedValue) {
    localStorage.setItem(key, encryptedValue);
  },

  LoadEncryptedData: function (key) {
    return localStorage.getItem(key) || "";
  },

  DeleteData: function (key) {
    localStorage.removeItem(key);
  },
};

mergeInto(LibraryManager.library, WebGLEncryptedStorage);
