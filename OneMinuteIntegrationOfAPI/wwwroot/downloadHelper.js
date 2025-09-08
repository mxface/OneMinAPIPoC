window.downloadHelper = {
    // Download arbitrary text as a file
    saveText: function (content, fileName, mime) {
        try {
            const blob = new Blob([content], { type: (mime || "text/plain") + ";charset=utf-8" });
            const url = URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = fileName || "download.txt";
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
            return true;
        } catch (e) {
            console.error("downloadHelper.saveText error:", e);
            return false;
        }
    },

    // Download the file the app serves at a given URL
    fromUrl: function (url, fileName) {
        try {
            const a = document.createElement("a");
            a.href = url;
            a.setAttribute("download", fileName || "");
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            return true;
        } catch (e) {
            console.error("downloadHelper.fromUrl error:", e);
            return false;
        }
    }
};
