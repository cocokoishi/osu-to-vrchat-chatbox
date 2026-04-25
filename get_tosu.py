import asyncio
import websockets
import json

async def main():
    async with websockets.connect('ws://127.0.0.1:24050/websocket/v2') as ws:
        msg = await ws.recv()
        with open('tosu_data.json', 'w', encoding='utf-8') as f:
            f.write(msg)
        print('Saved')

asyncio.run(main())
