window.copyHelper = {
    copyText: function (text) {
        if (!navigator.clipboard || !navigator.clipboard.writeText) {
            // Fallback for older browsers
            try {
                const ta = document.createElement('textarea');
                ta.value = text;
                ta.style.position = 'fixed';
                ta.style.top = '-1000px';
                document.body.appendChild(ta);
                ta.focus();
                ta.select();
                const ok = document.execCommand('copy');
                document.body.removeChild(ta);
                return ok; // boolean
            } catch {
                return false;
            }
        }
        return navigator.clipboard.writeText(text)
            .then(() => true)
            .catch(() => false);
    }
};
