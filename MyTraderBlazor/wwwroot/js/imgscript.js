window.showImageLightbox = function (imageUrl) {
    // Открывает лайтбокс с указанным URL изображения
    // В этом примере используется CSS-класс 'lightbox' для стилизации лайтбокса
    const lightbox = document.createElement('div');
    lightbox.classList.add('lightbox');

    const img = document.createElement('img');
    img.src = imageUrl;
    lightbox.appendChild(img);

    lightbox.addEventListener('click', () => {
        // Закрывает лайтбокс при клике на него
        lightbox.remove();
    });

    document.body.appendChild(lightbox);
};