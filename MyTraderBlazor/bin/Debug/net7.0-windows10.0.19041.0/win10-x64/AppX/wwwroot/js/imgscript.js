window.showImageLightbox = function (imageUrl) {
    // ��������� �������� � ��������� URL �����������
    // � ���� ������� ������������ CSS-����� 'lightbox' ��� ���������� ���������
    const lightbox = document.createElement('div');
    lightbox.classList.add('lightbox');

    const img = document.createElement('img');
    img.src = imageUrl;
    lightbox.appendChild(img);

    lightbox.addEventListener('click', () => {
        // ��������� �������� ��� ����� �� ����
        lightbox.remove();
    });

    document.body.appendChild(lightbox);
};