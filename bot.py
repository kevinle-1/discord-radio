import yaml
import discord
from discord.ext import tasks, commands
from discord_slash import SlashCommand
from discord_slash.utils.manage_commands import create_option

with open("conf.yaml", "r") as c: cfg = yaml.safe_load(c)
bot = commands.Bot(command_prefix="::")
slash = SlashCommand(bot, sync_commands=True)

# Slash Command
@slash.slash(name="radio",
             description="Load a radio station")
async def radio(ctx):


@bot.event
async def on_ready():
    print(f"Logged in as {bot.user.name}")

if __name__ == "__main__":
    bot.run(cfg["secrets"]["discord"]["token"])