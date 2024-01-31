import { CarAPI } from "/assets/js/carAPI.js"

window.onload = (e) => {
    document.getElementById('get-all-cars-button')?.addEventListener('click', LoadTable);
    document.getElementById('clear-all-cars-button')?.addEventListener('click', ClearTable);
    document.getElementById('home-button')?.addEventListener('click', () => { window.location.href = '/index.html' });
    ClearTable();
}

async function LoadTable() {
    const cars = await CarAPI.GetAllCars();
    if(!cars) {
        console.error('Could not load cars.')
        return;
    }

    const table = document.getElementById('car-table');
    if(!table) {
        console.error('Could not find car table.')
        return;
    }


    ClearTable();

    let table_body = table.getElementsByTagName('tbody')?.[0];
    if(!table_body) {
        console.error('Could not find <tbody> in car table!');
        return;
    }

    // Add each row manually
    cars.forEach(e => {
        const row = document.createElement('tr');
        row.addEventListener('dblclick', () => { DeleteCar(e.id) });

        const lstFeatures = e.features.join('<br>')

        row.innerHTML = `
                <td>${e.id}</td>
                <td>${e.manufacturer}</td>
                <td>${e.model}</td>
                <td>${e.price}</td>
                <td>${e.manufacturingDate}</td>
                <td>${e.quantityInStock}</td>
                <td>${e.isAvailable}</td>
                <td>${lstFeatures}</td>
                
        `
        table_body.appendChild(row)
    });

}

function ClearTable() {
    const table = document.getElementById('car-table');
    if(!table) {
        console.error('Could not find car table.')
        return;
    }
    table.innerHTML = `
    <thead>
        <tr>
            <th>ID</th>
            <th>Manufacturer</th>
            <th>Model</th>
            <th>Price</th>
            <th>Manufacturing Date</th>
            <th>Quantity in Stock</th>
            <th>Availability</th>
            <th>Features</th>
        </tr>
    </thead>
    <tbody>

    </tbody>
    `;
}

export function DeleteCar(carId) {
    
    const userConfirmed = window.confirm(`Are you sure you want to delete the car with ID = ${carId}?`);

    if (userConfirmed) {
        CarAPI.DeleteCar(carId);
        ClearTable();
        LoadTable();
    } else {
        alert(`Deletion of car with ID = ${carId} aborted.`);
    }
}