function copyToClipboard(text) {
    // Create a temporary textarea element
    const textarea = document.createElement('textarea');
    textarea.value = text;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';
    document.body.appendChild(textarea);

    // Select and copy the text
    textarea.select();
    let success = false;
    try {
        success = document.execCommand('copy');
        if (!success) {
            // Fallback to clipboard API if execCommand fails
            navigator.clipboard.writeText(text).then(() => {
                success = true;
            }).catch((err) => {
                console.error('Failed to copy:', err);
                success = false;
            });
        }
    } catch (err) {
        console.error('Failed to copy:', err);
        success = false;
    } finally {
        // Clean up
        document.body.removeChild(textarea);
    }
    return success;
}