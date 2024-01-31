import { CarAPI } from "/assets/js/carAPI.js"

window.onload = (e) => {
    document.getElementById('home-button')?.addEventListener('click', () => { window.location.href = '/index.html' });
    document.getElementById('clear-all-fields-button')?.addEventListener('click', OnClearButtonClick);
    document.getElementById('send-car-button')?.addEventListener('click', OnSendCarButonClick);
}

function OnClearButtonClick() {
    document.getElementById('manufacturer').value = '';
    document.getElementById('model').value = '';
    document.getElementById('price').value = '';
    document.getElementById('manufacturingDate').value = '';
    document.getElementById('quantityInStock').value = '';
    document.getElementById('availability').value = 'Yes';
    resetCheckboxes();
}

function resetCheckboxes() {

    const checkboxes = document.querySelectorAll('.new-car-form input[type="checkbox"]');

    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });
}

async function OnSendCarButonClick() {
    let car = { features: [] };

    const manufacturer = document.getElementById('manufacturer');
    if(!manufacturer.value) {
        alert('Manufacturer field is empty!')
        return;
    }
    car.manufacturer = manufacturer.value;

    const model = document.getElementById('model');
    if(!model.value) {
        alert('Model field is empty!')
        return;
    }
    car.model = model.value;

    const price = document.getElementById('price');
    if(!price.value) {
        alert('Price field is empty!')
        return;
    }
    car.price = price.value;

    const manufacturingDate = document.getElementById('manufacturingDate');
    if(!manufacturingDate.value) {
        alert('Manufacturing date field is empty!')
        return;
    }
    car.manufacturingDate = manufacturingDate.value;

    const quantityInStock = document.getElementById('quantityInStock');
    if(quantityInStock.value) {
        car.quantityInStock = quantityInStock.value;
    }

    const availability = document.getElementById('availability');
    if(availability) {
        car.isAvailable = availability.value;
    }
   
    const features = document.querySelectorAll('.form-check-input:checked');
    if (features.length > 0) {
        features.forEach(feature => {
            car.features.push(feature.value);
        });
    }

    const success = await CarAPI.CreateNewCar(car);
    if(success) {
        alert('Car successfully sent')
        OnClearButtonClick();
    }
    
}