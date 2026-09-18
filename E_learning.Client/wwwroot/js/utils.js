// Utility functions
console.log('utils.js loaded');

window.utils = {
    downloadTextFile: function(fileName, content, contentType) {
        const blob = new Blob([content], { type: contentType || 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');

        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
        URL.revokeObjectURL(url);
    },
    formatDate: function(date) {
        return new Date(date).toLocaleDateString('fr-FR');
    },
    formatNumber: function(num) {
        return num.toFixed(2);
    },
    debounce: function(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
};
