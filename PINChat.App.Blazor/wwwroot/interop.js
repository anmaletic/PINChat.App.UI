window.updateTextBoxHeight = (textBoxId) => {
    const textBox = document.getElementById(textBoxId);
    if (textBox) {
        textBox.style.height = "42px"; // Reset height to auto
        const scrollHeight = textBox.scrollHeight;
        textBox.style.height = `${scrollHeight}px`; // Set height to the scroll height
    }
};

window.resetTextBoxHeight = (textBoxId) => {
    const textBox = document.getElementById(textBoxId);
    if (textBox) {
        textBox.style.height = "40px"; // Reset height to auto
    }
};

window.scrollToBottomAuto = function(elementId) {
    const element = document.getElementById(elementId);
    let isAtBottom = element.scrollHeight - element.clientHeight <= element.scrollTop + 1;
    if (isAtBottom) {
        requestAnimationFrame(() => {
            element.scrollTop = element.scrollHeight;
        });
    }
};

window.scrollToBottomManual = function(elementId) {
    const element = document.getElementById(elementId);
    requestAnimationFrame(() => {
        element.scrollTop = element.scrollHeight;
    });
};

window.blazorHelpers = {
    triggerClick: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.click();
        }
    }
};

